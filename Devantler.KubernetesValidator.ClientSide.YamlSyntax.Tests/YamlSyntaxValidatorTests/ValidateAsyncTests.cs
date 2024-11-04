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
    var validator = new YamlSyntaxValidator();
    string directoryPath = Path.Combine(AppContext.BaseDirectory, "assets/k8s-invalid");
    var cancellationToken = new CancellationToken();

    // Act
    var task = new Func<Task>(() => validator.ValidateAsync(directoryPath, cancellationToken));

    // Assert
    _ = await Assert.ThrowsAsync<YamlSyntaxValidatorException>(task);
  }

  /// <summary>
  /// Tests the ValidateAsync method with invalid directory path throws exception.
  /// </summary>
  /// <returns></returns>
  [Fact]
  public async Task ValidateAsync_WithInvalidDirectoryPath_ShouldThrowYamlSyntaxValidatorException()
  {
    // Arrange
    var validator = new YamlSyntaxValidator();
    string directoryPath = Path.Combine(AppContext.BaseDirectory, "assets/invalid");
    var cancellationToken = new CancellationToken();

    // Act
    var task = new Func<Task>(() => validator.ValidateAsync(directoryPath, cancellationToken));

    // Assert
    var ex = await Assert.ThrowsAsync<YamlSyntaxValidatorException>(task);
    Assert.Contains("Could not find a part of the path", ex.Message, StringComparison.OrdinalIgnoreCase);
  }
}
