using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace H.Generators;

public static class CommonSteps
{
    public static IncrementalValuesProvider<GeneratorAttributeSyntaxContext>
        ForAttributeWithMetadataName(
            this SyntaxValueProvider source,
            string fullyQualifiedMetadataName)
    {
        return source
            .ForAttributeWithMetadataName(
                fullyQualifiedMetadataName: fullyQualifiedMetadataName,
                predicate: static (node, _) =>
                    node is
                        ClassDeclarationSyntax { AttributeLists.Count: > 0 } or 
                        RecordDeclarationSyntax { AttributeLists.Count: > 0 } or
                        InterfaceDeclarationSyntax { AttributeLists.Count: > 0 },
                transform: static (context, _) => context);
    }

    public static IncrementalValuesProvider<(SemanticModel SemanticModel, AttributeData AttributeData, TypeDeclarationSyntax TypeDeclarationSyntax, INamedTypeSymbol ClassSymbol)>
        SelectManyAllAttributesOfCurrentClassSyntax(
        this IncrementalValuesProvider<GeneratorAttributeSyntaxContext> source)
    {
        return source
            .SelectMany(static (context, _) => context.Attributes
                .Where(x =>
                {
                    var classSyntax = (TypeDeclarationSyntax)context.TargetNode;
                    var attributeSyntax = classSyntax.TryFindAttributeSyntax(x);
                    
                    return attributeSyntax != null;
                })
                .Select(x => (
                    context.SemanticModel,
                    AttributeData: x,
                    ClassSyntax: (TypeDeclarationSyntax)context.TargetNode,
                    ClassSymbol: (INamedTypeSymbol)context.TargetSymbol)));
    }

    internal static AttributeSyntax? TryFindAttributeSyntax(this TypeDeclarationSyntax typeDeclarationSyntax, AttributeData attribute)
    {
        var name = attribute.ConstructorArguments.ElementAtOrDefault(0).Value?.ToString();
        
        return typeDeclarationSyntax.AttributeLists
            .SelectMany(static x => x.Attributes)
            .FirstOrDefault(x => x.ArgumentList?.Arguments.FirstOrDefault()?.ToString().Trim('"').RemoveNameof() == name);
    }
    
    public static string RemoveNameof(this string value)
    {
        value = value ?? throw new ArgumentNullException(nameof(value));
        
        return value.Contains("nameof(")
            ? value
                .Substring(value.LastIndexOf('.') + 1)
                .TrimEnd(')', ' ')
            : value;
    }
}
