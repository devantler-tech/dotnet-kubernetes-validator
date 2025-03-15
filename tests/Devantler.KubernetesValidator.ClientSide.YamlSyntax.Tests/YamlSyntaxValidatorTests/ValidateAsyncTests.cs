namespace Devantler.KubernetesValidator.ClientSide.YamlSyntax.Tests.YamlSyntaxValidatorTests;

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
    var validator = new YamlSyntaxValidator();
    string directoryPath = Path.Combine(AppContext.BaseDirectory, "assets/k8s-valid");
    var cancellationToken = new CancellationToken();

    // Act
    var (isValid, message) = await validator.ValidateAsync(directoryPath, cancellationToken: cancellationToken);

    // Assert
    Assert.True(isValid);
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
    var validator = new YamlSyntaxValidator();
    string directoryPath = Path.Combine(AppContext.BaseDirectory, "assets/k8s-invalid");
    var cancellationToken = new CancellationToken();

    // Act
    var (isValid, message) = await validator.ValidateAsync(directoryPath, cancellationToken: cancellationToken);

    // Assert
    Assert.False(isValid);
    Assert.Contains("apps/kustomization.yaml - While parsing a node, did not find expected node content.", message, StringComparison.Ordinal);
  }
}
