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
    var (result, message) = await validator.ValidateAsync(directoryPath, cancellationToken);

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
    var (result, message) = await validator.ValidateAsync(directoryPath, cancellationToken);

    // Assert
    Assert.False(result);
    Assert.NotEmpty(message);
  }
}
