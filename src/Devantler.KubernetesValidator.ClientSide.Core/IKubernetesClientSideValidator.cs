namespace Devantler.KubernetesValidator.ClientSide.Core;

/// <summary>
/// Interface for a Kubernetes client-side validator.
/// </summary>
public interface IKubernetesClientSideValidator
{
  /// <summary>
  /// Validates the specified directory path.
  /// </summary>
  /// <param name="directoryPath"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  Task<(bool, string)> ValidateAsync(string directoryPath, CancellationToken cancellationToken = default);
}
