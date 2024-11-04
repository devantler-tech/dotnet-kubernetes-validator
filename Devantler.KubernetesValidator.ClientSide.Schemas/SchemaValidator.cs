using System.Text;
using CliWrap;
using Devantler.CLIRunner;
using Devantler.KubeconformCLI;
using Devantler.KubernetesValidator.ClientSide.Core;

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
    string[] kubeconformFlags = ["-ignore-filename-pattern=.*\\.sops\\.yaml"];
    string[] kubeconformConfig = [
      "-strict",
      "-ignore-missing-schemas",
      "-schema-location",
      "default",
      "-schema-location",
      "https://raw.githubusercontent.com/datreeio/CRDs-catalog/main/{{.Group}}/{{.ResourceKind}}_{{.ResourceAPIVersion}}.json",
      "-verbose"
    ];
    string[] kustomizeFlags = ["--load-restrictor=LoadRestrictionsNone"];

    string clusterPath = $"{directoryPath}/clusters";
    if (!Directory.Exists(clusterPath))
    {
      throw new SchemaValidatorException($"'{clusterPath}' directory does not exist");
    }
    foreach (string file in Directory.GetFiles($"{directoryPath}/clusters", "*.yaml", SearchOption.AllDirectories))
    {
      try
      {
        await Kubeconform.RunAsync(file, kubeconformFlags, kubeconformConfig, cancellationToken).ConfigureAwait(false);
      }
      catch (KubeconformException e)
      {
        throw new SchemaValidatorException(e.Message);
      }
    }
    const string Kustomization = "kustomization.yaml";
    foreach (string kustomization in Directory.GetFiles(directoryPath, Kustomization, SearchOption.AllDirectories))
    {
      string kustomizationPath = kustomization.Replace(Kustomization, "", StringComparison.Ordinal);
      var stdOutBuffer = new StringBuilder();
      var stdErrBuffer = new StringBuilder();
      var kustomizeBuildCmd = KustomizeCLI.Kustomize.Command.WithArguments(["build", kustomizationPath, .. kustomizeFlags])
        .WithStandardOutputPipe(PipeTarget.ToStringBuilder(stdOutBuffer))
        .WithStandardErrorPipe(PipeTarget.ToStringBuilder(stdErrBuffer));
      var kubeconformCmd = Kubeconform.Command.WithArguments([.. kubeconformFlags, .. kubeconformConfig])
        .WithStandardOutputPipe(PipeTarget.ToStringBuilder(stdOutBuffer))
        .WithStandardErrorPipe(PipeTarget.ToStringBuilder(stdErrBuffer));
      var (exitCode, _) = await CLI.RunAsync(kustomizeBuildCmd | kubeconformCmd, silent: true, cancellationToken: cancellationToken).ConfigureAwait(false);
      if (exitCode != 0)
      {
        throw new SchemaValidatorException($"'{kustomization}' - {stdErrBuffer}");
      }
    }
    return true;
  }
}
