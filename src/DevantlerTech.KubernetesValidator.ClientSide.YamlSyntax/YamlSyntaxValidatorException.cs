namespace DevantlerTech.KubernetesValidator.ClientSide.YamlSyntax;

/// <summary>
/// Represents an exception that is thrown when a YAML syntax validation error occurs.
/// </summary>
[Serializable]
public class YamlSyntaxValidatorException : Exception
{
  /// <summary>
  /// Initializes a new instance of the <see cref="YamlSyntaxValidatorException"/> class.
  /// </summary>
  public YamlSyntaxValidatorException()
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="YamlSyntaxValidatorException"/> class with a specified error message.
  /// </summary>
  /// <param name="message"></param>
  public YamlSyntaxValidatorException(string? message) : base(message)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="YamlSyntaxValidatorException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
  /// </summary>
  /// <param name="message"></param>
  /// <param name="innerException"></param>
  public YamlSyntaxValidatorException(string? message, Exception? innerException) : base(message, innerException)
  {
  }
}
