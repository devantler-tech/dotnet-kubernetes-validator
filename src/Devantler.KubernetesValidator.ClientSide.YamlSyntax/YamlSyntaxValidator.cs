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
  public async Task<(bool, string)> ValidateAsync(string directoryPath, CancellationToken cancellationToken = default)
  {
    try
    {
      var manifestPaths = Directory.GetFiles(directoryPath, "*.yaml", SearchOption.AllDirectories);
      var validationTasks = manifestPaths.Select(manifestPath =>
      {
        cancellationToken.ThrowIfCancellationRequested();
        using var input = File.OpenText(manifestPath);
        var parser = new Parser(input);
        _ = parser.Consume<StreamStart>();
        var deserializer = new Deserializer();
        while (parser.Accept<DocumentStart>(out var @event))
        {
          _ = deserializer.Deserialize(parser);
        }

        return Task.CompletedTask;
      });

      await Task.WhenAll(validationTasks).ConfigureAwait(false);
      return (true, string.Empty);
    }
#pragma warning disable CA1031 // Do not catch general exception types
    catch (Exception e)
    {
      return (false, $"{directoryPath} - {e.Message}");
    }
#pragma warning restore CA1031 // Do not catch general exception types
  }
}
