using H.Generators.Extensions;

namespace H.Generators;

internal class SourceGenerationHelper
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
        public event {GenerateEventHandlerType(@class, @event)}? {@event.Name};

        /// <summary>
        /// A helper method to raise the {@event.Name} event.
        /// </summary>
        {GenerateOnEventModifier(@class)} {GenerateEventArgsType(@class, @event)} On{@event.Name}({GenerateOnEventParameters(@class, @event)})
        {{
            {GenerateArgs(@class, @event)}
            {@event.Name}?.Invoke(this, args);

            return args;
        }}
    }}
}}".RemoveBlankLinesWhereOnlyWhitespaces();
    }

    public static string GenerateArgs(ClassData @class, EventData @event)
    {
        if (@event.Type != null)
        {
            return string.Empty;
        }

        return $"var args = new {GenerateEventArgsType(@class, @event)}();";
    }

    public static string GenerateOnEventModifier(ClassData @class)
    {
        if (@class.IsSealed)
        {
            return "private";
        }

        return "protected virtual";
    }

    public static string GenerateOnEventParameters(ClassData @class, EventData @event)
    {
        if (@event.Type == null)
        {
            return string.Empty;
        }

        return $"{GenerateEventArgsType(@class, @event)} args";
    }

    public static string GenerateType(string fullTypeName, bool isSpecialType)
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

    public static string GenerateEventArgsType(ClassData @class, EventData @event)
    {
        if (@event.Type != null)
        {
            return GenerateType(@event.Type, false);
        }

        return GenerateType("EventArgs");
    }

    public static string GenerateEventHandlerType(ClassData @class, EventData @event)
    {
        if (@event.Type != null)
        {
            return $"{GenerateType("EventHandler")}<{GenerateType(@event.Type, false)}>";
        }

        return GenerateType("EventHandler");
    }

    public static string GenerateType(string name)
    {
        return $"System.{name}".WithGlobalPrefix();
    }

    public static string GenerateXmlDocumentationFrom(string value)
    {
        var lines = value.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        return string.Join(Environment.NewLine, lines.Select(static line => $"        /// {line}"));
    }

    public static string GenerateXmlDocumentationFrom(string? value, EventData @event)
    {
        value ??= @$"<summary>
{(@event.Description != null ? $"{@event.Description}" : " ")}
</summary>".RemoveBlankLinesWhereOnlyWhitespaces();

        return GenerateXmlDocumentationFrom(value);
    }
}