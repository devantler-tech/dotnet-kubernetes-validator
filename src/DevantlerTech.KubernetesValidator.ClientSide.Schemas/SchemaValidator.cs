﻿using CliWrap;
using CliWrap.Buffered;
using DevantlerTech.KubeconformCLI;
using DevantlerTech.KubernetesValidator.ClientSide.Core;
using DevantlerTech.KustomizeCLI;

namespace DevantlerTech.KubernetesValidator.ClientSide.Schemas;

/// <summary>
/// Validator for Kubernetes schemas.
/// </summary>
public class SchemaValidator : IKubernetesClientSideValidator
{
  /// <summary>
  /// Validates that all YAML files in the specified directory conform to their upstream schemas.
  /// </summary>
  /// <param name="directoryPath"></param>
  /// <param name="ignore"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  /// <exception cref="NotImplementedException"></exception>
  public async Task<(bool, string)> ValidateAsync(string directoryPath, string[]? ignore = default, CancellationToken cancellationToken = default)
  {
    ignore ??= [@".+\.enc\.(yaml|yml)$"];
    string[] kubeconformFlags = [.. ignore.Select(pattern => $"-ignore-filename-pattern={pattern}")];
    string[] kubeconformConfig = [
      "-ignore-missing-schemas",
      "-schema-location",
      "default",
      "-schema-location",
      "https://raw.githubusercontent.com/datreeio/CRDs-catalog/main/{{.Group}}/{{.ResourceKind}}_{{.ResourceAPIVersion}}.json",
      "-verbose"
    ];
    string[] kustomizeFlags = ["--load-restrictor=LoadRestrictionsNone"];

    if (!Directory.Exists(directoryPath))
    {
      return (false, $"directory '{directoryPath}' does not exist");
    }
    else
    {
      var (isValid, message) = await ValidateSchemas(directoryPath, kubeconformFlags, kubeconformConfig, cancellationToken).ConfigureAwait(false);
    }
    return await ValidateKustomizations(directoryPath, kubeconformFlags, kubeconformConfig, kustomizeFlags, cancellationToken).ConfigureAwait(false);
  }

  static async Task<(bool, string)> ValidateSchemas(string directoryPath, string[] kubeconformFlags, string[] kubeconformConfig, CancellationToken cancellationToken)
  {
    var validationTasks = Directory.GetFiles(directoryPath, "*.yaml", SearchOption.AllDirectories).Select(async file =>
    {
      var arguments = kubeconformFlags.Concat(kubeconformConfig).Concat([file]);
      try
      {
        _ = await Kubeconform.RunAsync([.. arguments], silent: true, cancellationToken: cancellationToken).ConfigureAwait(false);
        return (true, string.Empty);
      }
#pragma warning disable CA1031 // Do not catch general exception types
      catch (Exception ex)
      {
        return (false, $"{file} - {ex.Message}");
      }
#pragma warning restore CA1031 // Do not catch general exception types
    });
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
    var results = await Task.WhenAll(validationTasks.Select(task => task.ContinueWith(t => t.Result, cts.Token, TaskContinuationOptions.None, TaskScheduler.Default))).ConfigureAwait(false);
    return results.FirstOrDefault(result => !result.Item1) is (bool, string) invalidResult ? invalidResult : (true, string.Empty);
  }

  static async Task<(bool, string)> ValidateKustomizations(string directoryPath, string[] kubeconformFlags, string[] kubeconformConfig, string[] kustomizeFlags, CancellationToken cancellationToken)
  {
    const string Kustomization = "kustomization.yaml";
    var kustomizationTasks = Directory.GetFiles(directoryPath, Kustomization, SearchOption.AllDirectories)
      .Select(async kustomization =>
      {
        string contents = await File.ReadAllTextAsync(kustomization, cancellationToken).ConfigureAwait(false);
        if (!(contents.Contains("apiVersion: kustomize.config.k8s.io/v1beta1", StringComparison.Ordinal) &&
          contents.Contains("kind: Kustomization", StringComparison.Ordinal)))
        {
          return (true, string.Empty);
        }
        string kustomizationPath = kustomization.Replace(Kustomization, "", StringComparison.Ordinal);
        var kustomizeBuildCmd = Kustomize.GetCommand().WithArguments(new[] { "build", kustomizationPath }.Concat(kustomizeFlags))
          .WithValidation(CommandResultValidation.None);
        var kubeconformCmd = Kubeconform.GetCommand().WithArguments(kubeconformFlags.Concat(kubeconformConfig))
          .WithValidation(CommandResultValidation.None);
        var kustomizeCommand = kustomizeBuildCmd;

        var result = await kustomizeCommand.ExecuteBufferedAsync(cancellationToken).ConfigureAwait(false);
        if (result.ExitCode != 0)
        {
          return (false, $"{Path.Combine(kustomizationPath, Kustomization)} - {result.StandardOutput + result.StandardError}");
        }
        var kubeconformCommand = result.StandardOutput | kubeconformCmd;
        result = await kubeconformCommand.ExecuteBufferedAsync(cancellationToken).ConfigureAwait(false);
        return result.ExitCode != 0
            ? (false, $"{Path.Combine(kustomizationPath, Kustomization)} - {result.StandardOutput + result.StandardError}")
            : (true, string.Empty);
      });
    var results = await Task.WhenAll(kustomizationTasks).ConfigureAwait(false);
    return results.FirstOrDefault(result => !result.Item1) is (bool, string) invalidResult ? invalidResult : (true, string.Empty);
  }
}
