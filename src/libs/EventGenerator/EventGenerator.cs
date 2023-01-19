using System.Collections.Immutable;
using System.Diagnostics;
using H.Generators.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using EventAttribute = EventGenerator.EventAttribute;

namespace H.Generators;

[Generator]
public class EventGenerator : IIncrementalGenerator
{
    #region Constants

    private const string Name = nameof(EventGenerator);
    private const string Id = "EG";

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
                    if (@event.Types.Any() &&
                        !@event.IsEventArgs)
                    {
                        context.AddTextSource(
                            hintName: $"{@class.Name}.EventArgs.{@event.Name}EventArgs.generated.cs",
                            text: SourceGenerationHelper.GenerateEventArgs(@class, @event));
                    }
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
                @class, cancellationToken) is not { } classSymbol)
            {
                continue;
            }

            var fullClassName = classSymbol.ToString();
            var @namespace = fullClassName.Substring(0, fullClassName.LastIndexOf('.'));
            var className = fullClassName.Substring(fullClassName.LastIndexOf('.') + 1);
            var classModifiers = SyntaxFacts.GetText(classSymbol.DeclaredAccessibility);
            classModifiers += classSymbol.IsStatic ? " static" : string.Empty;
            
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
                var name = attributeSyntax.ArgumentList?.Arguments[0].ToFullString().Trim('"') ?? string.Empty;
                if (name.Contains("nameof("))
                {
                    name = name
                        .Substring(name.LastIndexOf('.') + 1)
                        .TrimEnd(')', ' ');
                }
                var attribute = attributes[name];
                var attributeClass = attribute.AttributeClass?.ToDisplayString() ?? string.Empty;
                if (attributeClass.StartsWith(EventAttributeFullName, StringComparison.InvariantCulture))
                {
                    var propertyNamesConstant = GetPropertyFromAttributeData(attribute, nameof(EventAttribute.PropertyNames));
                    var propertyNames =
                        propertyNamesConstant is { IsNull: false }
                            ? GetPropertyFromAttributeData(attribute, nameof(EventAttribute.PropertyNames))?.Values
                                .Select(argument => argument.Value?.ToString() ?? string.Empty)
                                .ToArray() ?? Array.Empty<string>()
                            : Array.Empty<string>();
                    var types =
                        attribute.AttributeClass?.TypeArguments
                            .Select((argument, i) => new TypeData(
                                FullName: argument.ToDisplayString(),
                                IsSpecial: argument.SpecialType != SpecialType.None,
                                PropertyName: propertyNames.ElementAtOrDefault(i)?.ToPropertyName() ?? $"Value{i + 1}",
                                ParameterName: propertyNames.ElementAtOrDefault(i)?.ToParameterName() ?? $"value{i + 1}"))
                            .ToArray() ??
                        GetPropertyFromAttributeData(attribute, nameof(EventAttribute.Types))?.Values
                            .Select((argument, i) => new TypeData(
                                FullName: argument.ToString(),
                                IsSpecial: argument.Kind == TypedConstantKind.Primitive,
                                PropertyName: propertyNames.ElementAtOrDefault(i)?.ToPropertyName() ?? $"Value{i + 1}",
                                ParameterName: propertyNames.ElementAtOrDefault(i)?.ToParameterName() ?? $"value{i + 1}"))
                            .ToArray() ??
                        Array.Empty<TypeData>();
                    if (types.Length == 1)
                    {
                        types = types
                            .Select(static value => value with
                            {
                                PropertyName = value.PropertyName.Replace("1", string.Empty),
                                ParameterName = value.ParameterName.Replace("1", string.Empty),
                            })
                            .ToArray();
                    }
                    if (propertyNames.Any())
                    {
                        types = types.Zip(propertyNames, static (value, propertyName) => value with
                            {
                                PropertyName = propertyName.ToPropertyName(),
                                ParameterName = propertyName.ToParameterName(),
                            })
                            .ToArray();
                    }

                    var description = GetPropertyFromAttributeData(attribute, nameof(EventAttribute.Description))?.Value?.ToString();
                    var xmlDocumentation = GetPropertyFromAttributeData(attribute, nameof(EventAttribute.XmlDocumentation))?.Value?.ToString();
                    var isStatic = GetPropertyFromAttributeData(attribute, nameof(EventAttribute.IsStatic))?.Value?.ToString() ?? bool.FalseString;

                    var value = new EventData(
                        Name: name,
                        Types: types,
                        Description: description,
                        XmlDocumentation: xmlDocumentation,
                        IsStatic: bool.Parse(isStatic));
                    
                    events.Add(value);
                }
            }

            values.Add(new ClassData(
                Namespace: @namespace,
                Name: className,
                Modifiers: classModifiers,
                IsSealed: isSealed,
                Events: events));
        }

        return values;
    }

    private static bool IsGeneratorAttribute(string fullTypeName)
    {
        return
            fullTypeName.StartsWith(EventAttributeFullName, StringComparison.InvariantCulture);
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
        if (semanticModel.GetDeclaredSymbol(classDeclarationSyntax) is not { } classSymbol)
        {
            return null;
        }

        return classSymbol.ToString();
    }

    private static TypedConstant? GetPropertyFromAttributeData(AttributeData data, string name)
{
        return data.NamedArguments
            .FirstOrDefault(pair => pair.Key == name)
            .Value;
    }

    #endregion
}
