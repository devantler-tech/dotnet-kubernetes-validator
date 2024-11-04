namespace Devantler.KubernetesValidator.ClientSide.YamlSyntax.Tests;

/// <summary>
/// Tests for the <see cref="YamlSyntaxValidatorException"/> class.
/// </summary>
public class YamlSyntaxValidatorExceptionTests
{
  /// <summary>
  /// Tests the default constructor.
  /// </summary>
  [Fact]
  public void Constructor_GivenNothing_CallsDefaultConstructor()
  {
    // Act
    var exception = new YamlSyntaxValidatorException();

    // Assert
    Assert.NotNull(exception);
  }

  /// <summary>
  /// Tests the constructor with a message.
  /// </summary>
  [Fact]
  public void Constructor_GivenMessage_SetsMessageProperty()
  {
    // Arrange
    string message = "Test message";

    // Act
    var exception = new YamlSyntaxValidatorException(message);

    // Assert
    Assert.Equal(message, exception.Message);
  }

  /// <summary>
  /// Tests the constructor with a message and inner exception.
  /// </summary>
  [Fact]
  public void Constructor_GivenMessageAndInnerException_SetsMessageAndInnerExceptionProperties()
  {
    // Arrange
    string message = "Test message";
    var innerException = new NotImplementedException();

    // Act
    var exception = new YamlSyntaxValidatorException(message, innerException);

    // Assert
    Assert.Equal(message, exception.Message);
    Assert.Equal(innerException, exception.InnerException);
  }
}

