using System.Collections.Immutable;
using H.Generators.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using EventAttribute = EventGenerator.EventAttribute;

namespace H.Generators;

[Generator]
public class EventGenerator : IIncrementalGenerator
{
    #region Constants

    private const string Id = "EG";

    #endregion

    #region Methods

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.SyntaxProvider
            .ForAttributeWithMetadataName("EventGenerator.EventAttribute")
            .SelectManyAllAttributesOfCurrentClassSyntax()
            .SelectAndReportExceptions(PrepareData, context, Id)
            .WhereNotNull()
            .SelectAndReportExceptions(GetSourceCode, context, Id)
            .AddSource(context);
        context.SyntaxProvider
            .ForAttributeWithMetadataName("EventGenerator.EventAttribute`1")
            .SelectManyAllAttributesOfCurrentClassSyntax()
            .SelectAndReportExceptions(PrepareData, context, Id)
            .WhereNotNull()
            .SelectAndReportExceptions(GetSourceCode, context, Id)
            .AddSource(context);
        context.SyntaxProvider
            .ForAttributeWithMetadataName("EventGenerator.EventAttribute`2")
            .SelectManyAllAttributesOfCurrentClassSyntax()
            .SelectAndReportExceptions(PrepareData, context, Id)
            .WhereNotNull()
            .SelectAndReportExceptions(GetSourceCode, context, Id)
            .AddSource(context);
        context.SyntaxProvider
            .ForAttributeWithMetadataName("EventGenerator.EventAttribute`3")
            .SelectManyAllAttributesOfCurrentClassSyntax()
            .SelectAndReportExceptions(PrepareData, context, Id)
            .WhereNotNull()
            .SelectAndReportExceptions(GetSourceCode, context, Id)
            .AddSource(context);
        context.SyntaxProvider
            .ForAttributeWithMetadataName("EventGenerator.EventAttribute`4")
            .SelectManyAllAttributesOfCurrentClassSyntax()
            .SelectAndReportExceptions(PrepareData, context, Id)
            .WhereNotNull()
            .SelectAndReportExceptions(GetSourceCode, context, Id)
            .AddSource(context);
        context.SyntaxProvider
            .ForAttributeWithMetadataName("EventGenerator.EventAttribute`5")
            .SelectManyAllAttributesOfCurrentClassSyntax()
            .SelectAndReportExceptions(PrepareData, context, Id)
            .WhereNotNull()
            .SelectAndReportExceptions(GetSourceCode, context, Id)
            .AddSource(context);
        context.SyntaxProvider
            .ForAttributeWithMetadataName("EventGenerator.EventAttribute`6")
            .SelectManyAllAttributesOfCurrentClassSyntax()
            .SelectAndReportExceptions(PrepareData, context, Id)
            .WhereNotNull()
            .SelectAndReportExceptions(GetSourceCode, context, Id)
            .AddSource(context);
        context.SyntaxProvider
            .ForAttributeWithMetadataName("EventGenerator.EventAttribute`7")
            .SelectManyAllAttributesOfCurrentClassSyntax()
            .SelectAndReportExceptions(PrepareData, context, Id)
            .WhereNotNull()
            .SelectAndReportExceptions(GetSourceCode, context, Id)
            .AddSource(context);
        context.SyntaxProvider
            .ForAttributeWithMetadataName("EventGenerator.EventAttribute`8")
            .SelectManyAllAttributesOfCurrentClassSyntax()
            .SelectAndReportExceptions(PrepareData, context, Id)
            .WhereNotNull()
            .SelectAndReportExceptions(GetSourceCode, context, Id)
            .AddSource(context);
        context.SyntaxProvider
            .ForAttributeWithMetadataName("EventGenerator.EventAttribute`9")
            .SelectManyAllAttributesOfCurrentClassSyntax()
            .SelectAndReportExceptions(PrepareData, context, Id)
            .WhereNotNull()
            .SelectAndReportExceptions(GetSourceCode, context, Id)
            .AddSource(context);
    }

    private static (ClassData Class, EventData EventData)? PrepareData(
        (SemanticModel SemanticModel, AttributeData AttributeData, ClassDeclarationSyntax ClassSyntax, INamedTypeSymbol ClassSymbol) tuple)
    {
        var (_, attribute, _, classSymbol) = tuple;
        
        var fullClassName = classSymbol.ToString();
        var @namespace = fullClassName.Substring(0, fullClassName.LastIndexOf('.'));
        var className = fullClassName.Substring(fullClassName.LastIndexOf('.') + 1);
        var classModifiers = SyntaxFacts.GetText(classSymbol.DeclaredAccessibility);
        classModifiers += classSymbol.IsStatic ? " static" : string.Empty;
        
        var isSealed = classSymbol.IsSealed;

        var name =
            attribute.ConstructorArguments.ElementAtOrDefault(0).Value?.ToString() ??
            string.Empty;
        var propertyNamesConstant = attribute.GetNamedArgument(nameof(EventAttribute.PropertyNames));
        var propertyNames =
            propertyNamesConstant is { IsNull: false }
                ? attribute.GetNamedArgument(nameof(EventAttribute.PropertyNames)).Values
                    .Select(argument => argument.Value?.ToString() ?? string.Empty)
                    .ToArray()
                : Array.Empty<string>();
        var types =
            attribute.AttributeClass?.TypeArguments
                .Select((argument, i) => new TypeData(
                    FullName: argument.ToDisplayString(),
                    IsSpecial: argument.SpecialType != SpecialType.None,
                    PropertyName: propertyNames.ElementAtOrDefault(i)?.ToPropertyName() ?? $"Value{i + 1}",
                    ParameterName: propertyNames.ElementAtOrDefault(i)?.ToParameterName() ?? $"value{i + 1}",
                    IsGeneric: argument.ToDisplayString().Contains('<') && argument.ToDisplayString().Contains('>')))
                .ToArray() ??
            attribute.GetNamedArgument(nameof(EventAttribute.Types)).Values
                .Select((argument, i) => new TypeData(
                    FullName: argument.ToString(),
                    IsSpecial: argument.Kind == TypedConstantKind.Primitive,
                    PropertyName: propertyNames.ElementAtOrDefault(i)?.ToPropertyName() ?? $"Value{i + 1}",
                    ParameterName: propertyNames.ElementAtOrDefault(i)?.ToParameterName() ?? $"value{i + 1}",
                    IsGeneric: argument.ToString().Contains('<') && argument.ToString().Contains('>')))
                .ToArray();
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

        var description = attribute.GetNamedArgument(nameof(EventAttribute.Description)).Value?.ToString();
        var xmlDocumentation = attribute.GetNamedArgument(nameof(EventAttribute.XmlDocumentation)).Value?.ToString();
        var isStatic = attribute.GetNamedArgument(nameof(EventAttribute.IsStatic)).Value?.ToString() ?? bool.FalseString;

        var eventData = new EventData(
            Name: name,
            Types: types,
            Description: description,
            XmlDocumentation: xmlDocumentation,
            IsStatic: bool.Parse(isStatic));
        var classData = new ClassData(
            Namespace: @namespace,
            Name: className,
            Modifiers: classModifiers,
            IsSealed: isSealed);
        
        return (classData, eventData);
    }
    
    private static EquatableArray<FileWithName> GetSourceCode((ClassData Class, EventData Event) data)
    {
        var className = data.Class.Name
            .Replace("<", "{")
            .Replace(">", "}");
        var files = new List<FileWithName>
        {
            new (
                Name: $"{className}.Events.{data.Event.Name}.generated.cs",
                Text: SourceGenerationHelper.GenerateEvent(data.Class, data.Event))
        };
        if (data.Event.Types.Any() &&
            !data.Event.IsEventArgs)
        {
            files.Add(new FileWithName(
                Name: $"{className}.EventArgs.{data.Event.Name}EventArgs.generated.cs",
                Text: SourceGenerationHelper.GenerateEventArgs(data.Class, data.Event)));
        }
        
        return files.ToImmutableArray().AsEquatableArray();
    }

    #endregion
}
