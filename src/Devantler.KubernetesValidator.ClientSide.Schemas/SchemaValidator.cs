using System.Text;
using CliWrap;
using CliWrap.Buffered;
using Devantler.KubeconformCLI;
using Devantler.KubernetesValidator.ClientSide.Core;
using Devantler.KustomizeCLI;

namespace Devantler.KubernetesValidator.ClientSide.Schemas;

/// <summary>
/// Validator for Kubernetes schemas.
/// </summary>
public class SchemaValidator : IKubernetesClientSideValidator
{
  /// <summary>
  /// Validates that all YAML files in the specified directory conform to their upstream schemas.
  /// </summary>
  /// <param name="directoryPath"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  /// <exception cref="NotImplementedException"></exception>
  public async Task<bool> ValidateAsync(string directoryPath, CancellationToken cancellationToken = default)
  {
    string[] ignoreFileNamePatterns = [@".+\.enc\.(yaml|yml)$"];
    string[] kubeconformFlags = [.. ignoreFileNamePatterns.Select(pattern => $"-ignore-filename-pattern={pattern}")];
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
      throw new SchemaValidatorException($"'{directoryPath}' directory does not exist");
    }
    else
    {
      foreach (string file in Directory.GetFiles(directoryPath, "*.yaml", SearchOption.AllDirectories))
      {
        var arguments = kubeconformFlags.Concat(kubeconformConfig).Concat([file]);
        await Kubeconform.RunAsync([.. arguments], silent: true, cancellationToken: cancellationToken).ConfigureAwait(false);
      }
    }
    const string Kustomization = "kustomization.yaml";
    foreach (string kustomization in Directory.GetFiles(directoryPath, Kustomization, SearchOption.AllDirectories))
    {
      var contents = await File.ReadAllTextAsync(kustomization, cancellationToken).ConfigureAwait(false);
      if (!(contents.Contains("apiVersion: kustomize.config.k8s.io/v1beta1", StringComparison.Ordinal) &&
        contents.Contains("kind: Kustomization", StringComparison.Ordinal)))
      {
        continue;
      }
      string kustomizationPath = kustomization.Replace(Kustomization, "", StringComparison.Ordinal);
      var stdOutBuffer = new StringBuilder();
      var stdErrBuffer = new StringBuilder();
      var kustomizeBuildCmd = Kustomize.Command.WithArguments(["build", kustomizationPath, .. kustomizeFlags])
        .WithStandardOutputPipe(PipeTarget.ToStringBuilder(stdOutBuffer))
        .WithStandardErrorPipe(PipeTarget.ToStringBuilder(stdErrBuffer));
      var kubeconformCmd = Kubeconform.Command.WithArguments([.. kubeconformFlags, .. kubeconformConfig])
        .WithStandardOutputPipe(PipeTarget.ToStringBuilder(stdOutBuffer))
        .WithStandardErrorPipe(PipeTarget.ToStringBuilder(stdErrBuffer));
      var command = kustomizeBuildCmd | kubeconformCmd;
      await command.ExecuteBufferedAsync(cancellationToken);
    }
    return true;
  }
}
