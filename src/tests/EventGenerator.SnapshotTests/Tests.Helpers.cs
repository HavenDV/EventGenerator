using Microsoft.CodeAnalysis;
using H.Generators.Tests.Extensions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Testing;

namespace H.Generators.SnapshotTests;

public partial class Tests : VerifyBase
{
    private static string GetHeader()
    {
        return @$"
using EventGenerator;

#nullable enable

namespace H.Generators.IntegrationTests;
";
    }

    public async Task CheckSourceAsync(
        string source,
        CancellationToken cancellationToken = default)
    {
        source = $"{GetHeader()}{source}";

        var referenceAssemblies = ReferenceAssemblies.Net.Net60;
        var references = await referenceAssemblies.ResolveAsync(null, cancellationToken);
        var compilation = (Compilation)CSharpCompilation.Create(
            assemblyName: "Tests",
            syntaxTrees: new[]
            {
                CSharpSyntaxTree.ParseText(source, cancellationToken: cancellationToken),
            },
            references: references
                .Add(MetadataReference.CreateFromFile(typeof(global::EventGenerator.EventAttribute).Assembly.Location)),
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        var generator = new EventGenerator();
        var globalOptions = new Dictionary<string, string>();
        var driver = CSharpGeneratorDriver
            .Create(generator)
            .WithUpdatedAnalyzerConfigOptions(new DictionaryAnalyzerConfigOptionsProvider(globalOptions))
            .RunGeneratorsAndUpdateCompilation(compilation, out compilation, out _, cancellationToken);
        var diagnostics = compilation.GetDiagnostics(cancellationToken);

        await Task.WhenAll(
            Verify(diagnostics)
                .UseDirectory("Snapshots")
                .UseTextForParameters("Diagnostics"),
            Verify(driver)
                .UseDirectory("Snapshots"));
    }
}