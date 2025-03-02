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
    bool result = await validator.ValidateAsync(directoryPath, cancellationToken);

    // Assert
    Assert.True(result);
  }

  /// <summary>
  /// Tests the ValidateAsync method with invalid YAML files.
  /// </summary>
  /// <returns></returns>
  [Fact]
  public async Task ValidateAsync_WithInvalidYamlFiles_ShouldThrowYamlSyntaxValidatorException()
  {
    // Arrange
    var validator = new SchemaValidator();
    string directoryPath = Path.Combine(AppContext.BaseDirectory, "assets/k8s-invalid");
    var cancellationToken = new CancellationToken();

    // Act
    var task = new Func<Task>(() => validator.ValidateAsync(directoryPath, cancellationToken));

    // Assert
    var ex = await Assert.ThrowsAsync<CommandExecutionException>(task);
  }
}
