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
  /// <param name="ignore"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  /// <exception cref="NotImplementedException"></exception>
  public async Task<(bool, string)> ValidateAsync(string directoryPath, string[]? ignore = default, CancellationToken cancellationToken = default)
  {
    var manifestPaths = Directory.GetFiles(directoryPath, "*.yaml", SearchOption.AllDirectories)
      .Where(path => ignore == null || !ignore.Any(ignorePattern =>
        path.Contains(ignorePattern, StringComparison.OrdinalIgnoreCase)))
      .ToArray();
    var validationTasks = manifestPaths.Select(async manifestPath => await Task.Run(() =>
    {
      try
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
      }
#pragma warning disable CA1031 // Do not catch general exception types
      catch (Exception ex)
      {
        var relativePath = Path.GetRelativePath(directoryPath, manifestPath);
        return (false, $"{relativePath} - {ex.Message}");
      }
#pragma warning restore CA1031 // Do not catch general exception types

      return (true, string.Empty);
    }).ConfigureAwait(false));

    var results = await Task.WhenAll(validationTasks).ConfigureAwait(false);
    return results.FirstOrDefault(result => !result.Item1) is (bool, string) invalidResult ? invalidResult : (true, string.Empty);

  }
}
