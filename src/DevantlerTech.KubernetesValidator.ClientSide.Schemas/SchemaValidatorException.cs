namespace DevantlerTech.KubernetesValidator.ClientSide.Schemas;

/// <summary>
/// Exception thrown when a schema validation error occurs.
/// </summary>
[Serializable]
public class SchemaValidatorException : Exception
{
  /// <summary>
  /// Initializes a new instance of the <see cref="SchemaValidatorException"/> class.
  /// </summary>
  public SchemaValidatorException()
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="SchemaValidatorException"/> class with a specified error message.
  /// </summary>
  /// <param name="message"></param>
  public SchemaValidatorException(string? message) : base(message)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="SchemaValidatorException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
  /// </summary>
  /// <param name="message"></param>
  /// <param name="innerException"></param>
  public SchemaValidatorException(string? message, Exception? innerException) : base(message, innerException)
  {
  }
}
