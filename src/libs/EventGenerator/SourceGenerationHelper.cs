using H.Generators.Extensions;
using Microsoft.CodeAnalysis;

namespace H.Generators;

internal class SourceGenerationHelper
{
    public static string GenerateEvent(ClassData @class, EventData @event)
    {
        return @$" 
#nullable enable

namespace {@class.Namespace}
{{
    public{@class.Modifiers} partial class {@class.Name}
    {{
{GenerateXmlDocumentationFrom(@event.EventXmlDocumentation, @event)}
        public event {GenerateEventHandlerType(@class)}? {@event.Name};

        /// <summary>
        /// A helper method to raise the {@event.Name} event.
        /// </summary>
        protected {GenerateEventArgsType(@class)} On{@event.Name}()
        {{
            var args = new {GenerateEventArgsType(@class)}();
            {@event.Name}?.Invoke(this, args);

            return args;
        }}
    }}
}}".RemoveBlankLinesWhereOnlyWhitespaces();
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

    public static string GenerateEventArgsType(ClassData @class)
    {
        return GenerateType("EventArgs");
    }

    public static string GenerateEventHandlerType(ClassData @class)
    {
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