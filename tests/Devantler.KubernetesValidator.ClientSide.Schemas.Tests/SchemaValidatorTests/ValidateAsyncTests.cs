using CliWrap.Exceptions;

namespace Devantler.KubernetesValidator.ClientSide.Schemas.Tests.SchemaValidatorTests;

/// <summary>
/// Tests for the ValidateAsync method.
/// </summary>
public class ValidateAsyncTests
{
  /// <summary>
  /// Tests the ValidateAsync method with valid YAML files.
  /// </summary>
  /// <returns></returns>
  [Fact]
  public async Task ValidateAsync_WithValidYamlFiles_ShouldReturnTrue()
  {
    // Arrange
    var validator = new SchemaValidator();
    string directoryPath = Path.Combine(AppContext.BaseDirectory, "assets/k8s-valid");
    var cancellationToken = new CancellationToken();

    // Act
    var (result, message) = await validator.ValidateAsync(directoryPath, cancellationToken: cancellationToken);

    // Assert
    Assert.True(result);
    Assert.Empty(message);
  }

  /// <summary>
  /// Tests the ValidateAsync method with invalid YAML files.
  /// </summary>
  /// <returns></returns>
  [Fact]
  public async Task ValidateAsync_WithInvalidYamlFiles_ShouldReturnFalse()
  {
    // Arrange
    var validator = new SchemaValidator();
    string directoryPath = Path.Combine(AppContext.BaseDirectory, "assets/k8s-invalid");
    var cancellationToken = new CancellationToken();

    // Act
    var (result, message) = await validator.ValidateAsync(directoryPath, cancellationToken: cancellationToken);

    // Assert
    Assert.False(result);
    Assert.Contains(
      $"clusters/ksail-default/flux-system/kustomization.yaml - Error: accumulating resources: accumulation err='accumulating resources from 'ddd': open " +
      $"{Path.Combine(AppContext.BaseDirectory, "assets/k8s-invalid/clusters/ksail-default/flux-system/ddd")}: no such file or directory': must build at directory: not a valid directory: evalsymlink failure on " +
      $"'{Path.Combine(AppContext.BaseDirectory, "assets/k8s-invalid/clusters/ksail-default/flux-system/ddd")}' : lstat " +
      $"{Path.Combine(AppContext.BaseDirectory, "assets/k8s-invalid/clusters/ksail-default/flux-system/ddd")}: no such file or directory\n",
      message, StringComparison.OrdinalIgnoreCase);
  }
}
