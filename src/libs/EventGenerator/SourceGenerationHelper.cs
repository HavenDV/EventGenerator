using H.Generators.Extensions;

namespace H.Generators;

internal static class SourceGenerationHelper
{
    public static string GenerateEvent(ClassData @class, EventData @event)
    {
        return @$" 
#nullable enable

namespace {@class.Namespace}
{{
    {@class.Modifiers} partial class {@class.Name}
    {{
{GenerateXmlDocumentationFrom(@event.XmlDocumentation, @event)}
        public event {GenerateEventHandlerType(@event)}? {@event.Name};

        /// <summary>
        /// A helper method to raise the {@event.Name} event.
        /// </summary>
        {GenerateOnEventModifier(@class)} {GenerateEventArgsType(@event)} On{@event.Name}({GenerateOnEventParameters(@event)})
        {{
            {GenerateArgs(@event)}
            {@event.Name}?.Invoke(this, args);

            return args;
        }}
    }}
}}".RemoveBlankLinesWhereOnlyWhitespaces();
    }

    private static string GenerateArgs(EventData @event)
    {
        if (@event.Type != null)
        {
            return string.Empty;
        }

        return $"var args = new {GenerateEventArgsType(@event)}();";
    }

    private static string GenerateOnEventModifier(ClassData @class)
    {
        if (@class.IsSealed)
        {
            return "private";
        }

        return "protected virtual";
    }

    private static string GenerateOnEventParameters(EventData @event)
    {
        if (@event.Type == null)
        {
            return string.Empty;
        }

        return $"{GenerateEventArgsType(@event)} args";
    }

    private static string GenerateType(string fullTypeName, bool isSpecialType)
    {
        if (isSpecialType)
        {
            return fullTypeName;
        }

        return fullTypeName switch
        {
            _ => fullTypeName.WithGlobalPrefix(),
        };
    }

    private static string GenerateEventArgsType(EventData @event)
    {
        if (@event.Type != null)
        {
            return GenerateType(@event.Type, false);
        }

        return GenerateType("EventArgs");
    }

    private static string GenerateEventHandlerType(EventData @event)
    {
        if (@event.Type != null)
        {
            return $"{GenerateType("EventHandler")}<{GenerateType(@event.Type, false)}>";
        }

        return GenerateType("EventHandler");
    }

    private static string GenerateType(string name)
    {
        return $"System.{name}".WithGlobalPrefix();
    }

    private static string GenerateXmlDocumentationFrom(string value)
    {
        var lines = value.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        return string.Join(Environment.NewLine, lines.Select(static line => $"        /// {line}"));
    }

    private static string GenerateXmlDocumentationFrom(string? value, EventData @event)
    {
        value ??= @$"<summary>
{(@event.Description != null ? $"{@event.Description}" : " ")}
</summary>".RemoveBlankLinesWhereOnlyWhitespaces();

        return GenerateXmlDocumentationFrom(value);
    }
}