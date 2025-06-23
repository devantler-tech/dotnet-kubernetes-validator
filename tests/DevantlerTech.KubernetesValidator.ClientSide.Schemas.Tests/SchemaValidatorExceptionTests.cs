namespace DevantlerTech.KubernetesValidator.ClientSide.Schemas.Tests;

/// <summary>
/// Tests for the <see cref="SchemaValidatorException"/> class.
/// </summary>
public class SchemaValidatorExceptionTests
{
  /// <summary>
  /// Tests the default constructor.
  /// </summary>
  [Fact]
  public void Constructor_GivenNothing_CallsDefaultConstructor()
  {
    // Act
    var exception = new SchemaValidatorException();

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
    var exception = new SchemaValidatorException(message);

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
    var exception = new SchemaValidatorException(message, innerException);

    // Assert
    Assert.Equal(message, exception.Message);
    Assert.Equal(innerException, exception.InnerException);
  }
}

