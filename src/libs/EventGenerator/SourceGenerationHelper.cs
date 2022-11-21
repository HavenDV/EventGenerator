﻿using H.Generators.Extensions;

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
{(@event.Types.Length <= 1 ? " " : $@"
        /// <summary>
        /// A helper method to raise the {@event.Name} event.
        /// </summary>
        {GenerateOnEventModifier(@class)} {GenerateEventArgsType(@class, @event)} On{@event.Name}(
{@event.Types.Select(static (type, i) => @$" 
            {GenerateType(type)} value{i + 1},
".RemoveBlankLinesWhereOnlyWhitespaces()).Inject().Trim(',')})
        {{
            var args = new {GenerateEventArgsType(@class, @event)}({string.Join(", ", @event.Types.Select(static (_, i) => $"value{i + 1}"))});
            {@event.Name}?.Invoke(this, args);

            return args;
        }}")}
    }}
}}".RemoveBlankLinesWhereOnlyWhitespaces();
    }

    public static string GenerateEventArgs(ClassData @class, EventData @event)
    {
        return @$" 
#nullable enable

namespace {@class.Namespace}
{{
    {@class.Modifiers} partial class {@class.Name}
    {{
        public class {@event.Name}EventArgs : global::System.EventArgs
        {{
{@event.Types.Select(static (type, i) => @$" 
            /// <summary>
            /// 
            /// </summary>
            public {GenerateType(type)} Value{i + 1} {{ get; }}

".RemoveBlankLinesWhereOnlyWhitespaces()).Inject()}

            /// <summary>
            /// 
            /// </summary>
            public {@event.Name}EventArgs({string.Join(", ", @event.Types.Select(static (type, i) => $"{GenerateType(type)} value{i + 1}"))})
            {{
{@event.Types.Select(static (_, i) => @$" 
                Value{i + 1} = value{i + 1};
".RemoveBlankLinesWhereOnlyWhitespaces()).Inject()}
            }}

            /// <summary>
            /// 
            /// </summary>
            public void Deconstruct({string.Join(", ", @event.Types.Select(static (type, i) => $"out {GenerateType(type)} value{i + 1}"))})
            {{
{@event.Types.Select(static (_, i) => @$" 
                value{i + 1} = Value{i + 1};
".RemoveBlankLinesWhereOnlyWhitespaces()).Inject()}
            }}
        }}
    }}
}}".RemoveBlankLinesWhereOnlyWhitespaces();
    }
    
    private static string GenerateArgs(ClassData @class, EventData @event)
    {
        if (@event.Types.Any())
        {
            return string.Empty;
        }

        return $"var args = new {GenerateEventArgsType(@class, @event)}();";
    }

    private static string GenerateOnEventModifier(ClassData @class)
    {
        if (@class.IsSealed)
        {
            return "private";
        }

        return "protected virtual";
    }

    private static string GenerateOnEventParameters(ClassData @class, EventData @event)
    {
        if (!@event.Types.Any())
        {
            return string.Empty;
        }

        return $"{GenerateEventArgsType(@class, @event)} args";
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

    private static string GenerateType(TypeData typeData)
    {
        return GenerateType(typeData.FullName, typeData.IsSpecial);
    }

    private static string GenerateSystemType(string name)
    {
        return GenerateType($"System.{name}", isSpecialType: false);
    }
    
    private static string GenerateEventArgsType(ClassData @class, EventData @event)
    {
        if (@event.Types.Length > 1)
        {
            return $"{GenerateType($"{@class.Namespace}.{@class.Name}.{@event.Name}EventArgs", false)}";
        }
        if (@event.Types.Length == 1)
        {
            return GenerateType(@event.Types[0]);
        }

        return GenerateSystemType("EventArgs");
    }

    private static string GenerateEventHandlerType(ClassData @class, EventData @event)
    {
        if (@event.Types.Any())
        {
            return $"{GenerateSystemType("EventHandler")}<{GenerateEventArgsType(@class, @event)}>";
        }

        return GenerateSystemType("EventHandler");
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