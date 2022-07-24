using System.Collections.Immutable;
using System.Diagnostics;
using H.Generators.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using EventAttribute = EventGenerator.EventAttribute;

namespace H.Generators;

[Generator]
public class EventGenerator : IIncrementalGenerator
{
    #region Constants

    public const string Name = nameof(EventGenerator);
    public const string Id = "EG";

    private static string EventAttributeFullName => typeof(EventAttribute).FullName;

    #endregion

    #region Methods

    private static ClassDeclarationSyntax? GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
    {
        var syntax = (ClassDeclarationSyntax)context.Node;

        return syntax
            .AttributeLists
            .SelectMany(static list => list.Attributes)
            .Any(attributeSyntax => IsGeneratorAttribute(attributeSyntax, context.SemanticModel))
            ? syntax
            : null;
    }

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var classes = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (node, _) => node is ClassDeclarationSyntax { AttributeLists.Count: > 0 },
                transform: static (context, _) => GetSemanticTargetForGeneration(context))
            .Where(static syntax => syntax is not null);

        var compilationAndClasses = context.CompilationProvider
            .Combine(context.AnalyzerConfigOptionsProvider)
            .Combine(classes.Collect());

        context.RegisterSourceOutput(
            compilationAndClasses,
            static (context, source) => Execute(source.Left.Left, source.Left.Right, source.Right!, context));
    }

    private static void Execute(
        Compilation compilation,
        AnalyzerConfigOptionsProvider options,
        ImmutableArray<ClassDeclarationSyntax> classSyntaxes,
        SourceProductionContext context)
    {
        if (!options.IsDesignTime() &&
            options.GetGlobalOption("DebuggerBreak", prefix: Name) != null)
        {
            Debugger.Launch();
        }
        
        if (classSyntaxes.IsDefaultOrEmpty)
        {
            return;
        }
        
        try
        {
            var classes = GetTypesToGenerate(compilation, classSyntaxes, context.CancellationToken);
            foreach (var @class in classes)
            {
                foreach (var @event in @class.Events)
                {
                    context.AddTextSource(
                        hintName: $"{@class.Name}.Events.{@event.Name}.generated.cs",
                        text: SourceGenerationHelper.GenerateEvent(@class, @event));
                }
            }
        }
        catch (Exception exception)
        {
            context.ReportException(
                id: "001",
                exception: exception,
                prefix: Id);
        }
    }

    private static IReadOnlyCollection<ClassData> GetTypesToGenerate(
        Compilation compilation,
        IEnumerable<ClassDeclarationSyntax> classes,
        CancellationToken cancellationToken)
    {
        var values = new List<ClassData>();
        foreach (var group in classes.GroupBy(@class => GetFullClassName(compilation, @class)))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var @class = group.First();
            
            var semanticModel = compilation.GetSemanticModel(@class.SyntaxTree);
            if (semanticModel.GetDeclaredSymbol(
                @class, cancellationToken) is not INamedTypeSymbol classSymbol)
            {
                continue;
            }

            var fullClassName = classSymbol.ToString();
            var @namespace = fullClassName.Substring(0, fullClassName.LastIndexOf('.'));
            var className = fullClassName.Substring(fullClassName.LastIndexOf('.') + 1);
            var classModifiers = classSymbol.IsStatic ? " static" : string.Empty;
            var isSealed = classSymbol.IsSealed;

            var events = new List<EventData>();
            var attributes = @classSymbol.GetAttributes()
                .Where(IsGeneratorAttribute)
                .Where(static attribute => attribute.ConstructorArguments.ElementAtOrDefault(0).Value is string)
                .ToDictionary(static attribute => attribute.ConstructorArguments[0].Value as string ?? string.Empty);
            foreach (var attributeSyntax in group
                .SelectMany(static list => list.AttributeLists)
                .SelectMany(static list => list.Attributes)
                .Where(attributeSyntax => IsGeneratorAttribute(attributeSyntax, compilation.GetSemanticModel(attributeSyntax.SyntaxTree))))
            {
                var name = attributeSyntax.ArgumentList?.Arguments[0].ToFullString()?.Trim('"') ?? string.Empty;
                if (name.Contains("nameof("))
                {
                    name = name
                        .Substring(name.LastIndexOf('.') + 1)
                        .TrimEnd(')', ' ');
                }
                var attribute = attributes[name];
                var attributeClass = attribute.AttributeClass?.ToDisplayString() ?? string.Empty;
                if (attributeClass.StartsWith(EventAttributeFullName))
                {
                    var type =
                        GetGenericTypeArgumentFromAttributeData(attribute, 0)?.ToDisplayString() ??
                        GetPropertyFromAttributeData(attribute, nameof(EventAttribute.Type))?.Value?.ToString();

                    var description = GetPropertyFromAttributeData(attribute, nameof(EventAttribute.Description))?.Value?.ToString();

                    var xmlDocumentation = GetPropertyFromAttributeData(attribute, nameof(EventAttribute.XmlDocumentation))?.Value?.ToString();

                    var value = new EventData(
                        Name: name,
                        Type: type,
                        Description: description,
                        XmlDocumentation: xmlDocumentation);
                    
                    events.Add(value);
                }
            }

            values.Add(new ClassData(
                Namespace: @namespace,
                Name: className,
                FullName: fullClassName,
                Modifiers: classModifiers,
                IsSealed: isSealed,
                Events: events));
        }

        return values;
    }

    private static bool IsGeneratorAttribute(string fullTypeName)
    {
        return
            fullTypeName.StartsWith(EventAttributeFullName);
    }

    private static bool IsGeneratorAttribute(AttributeSyntax attributeSyntax, SemanticModel semanticModel)
    {
        if (semanticModel.GetSymbolInfo(attributeSyntax).Symbol is not IMethodSymbol attributeSymbol)
        {
            return false;
        }

        var attributeContainingTypeSymbol = attributeSymbol.ContainingType;
        var fullName = attributeContainingTypeSymbol.ToDisplayString();

        return IsGeneratorAttribute(fullName);
    }

    private static bool IsGeneratorAttribute(AttributeData attributeData)
{
        var attributeClass = attributeData.AttributeClass?.ToDisplayString() ?? string.Empty;

        return IsGeneratorAttribute(attributeClass);
    }

    private static string? GetFullClassName(Compilation compilation, ClassDeclarationSyntax classDeclarationSyntax)
    {
        var semanticModel = compilation.GetSemanticModel(classDeclarationSyntax.SyntaxTree);
        if (semanticModel.GetDeclaredSymbol(classDeclarationSyntax) is not INamedTypeSymbol classSymbol)
        {
            return null;
        }

        return classSymbol.ToString();
    }

    private static bool? IsSpecialType(ITypeSymbol? symbol)
    {
        if (symbol == null)
        {
            return null;
        }

        return symbol.SpecialType != SpecialType.None;
    }

    private static ITypeSymbol? GetGenericTypeArgumentFromAttributeData(AttributeData data, int position)
    {
        return data.AttributeClass?.TypeArguments.ElementAtOrDefault(position);
    }

    private static TypedConstant? GetPropertyFromAttributeData(AttributeData data, string name)
{
        return data.NamedArguments
            .FirstOrDefault(pair => pair.Key == name)
            .Value;
    }

    private static string? GetPropertyFromAttributeSyntax(AttributeSyntax syntax, string name)
    {
        return syntax.ArgumentList?.Arguments
            .FirstOrDefault(syntax =>
            {
                var nameEquals = syntax.NameEquals?.ToFullString()?
                    .Trim('=', ' ', '\t', '\r', '\n');
                
                return nameEquals == name;
            })?
            .Expression
            .ToFullString();
    }

    #endregion
}
