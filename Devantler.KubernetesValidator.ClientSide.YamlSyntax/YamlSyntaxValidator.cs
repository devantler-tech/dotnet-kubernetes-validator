using Devantler.KubernetesValidator.ClientSide.Core;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Devantler.KubernetesValidator.ClientSide.YamlSyntax;

/// <summary>
/// Validator for YAML syntax.
/// </summary>
public class YamlSyntaxValidator : IKubernetesClientSideValidator
{
  /// <summary>
  /// Validates the specified directory path.
  /// </summary>
  /// <param name="directoryPath"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  /// <exception cref="NotImplementedException"></exception>
  public Task<bool> ValidateAsync(string directoryPath, CancellationToken cancellationToken = default)
  {
    try
    {
      foreach (string manifestPath in Directory.GetFiles(directoryPath, "*.yaml", SearchOption.AllDirectories))
      {
        try
        {
          var input = File.OpenText(manifestPath);
          var parser = new Parser(input);
          var deserializer = new Deserializer();
          _ = parser.Consume<StreamStart>();

          while (parser.Accept<DocumentStart>(out var @event))
          {
            object? doc = deserializer.Deserialize(parser);
          }
        }
        catch (YamlException e)
        {
          throw new YamlSyntaxValidatorException($"{manifestPath} - {e.Message}");
        }
      }
    }
    catch (DirectoryNotFoundException e)
    {
      throw new YamlSyntaxValidatorException(e.Message);
    }
    return Task.FromResult(true);
  }
}
