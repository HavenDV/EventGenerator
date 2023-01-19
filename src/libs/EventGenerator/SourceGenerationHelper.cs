using H.Generators.Extensions;

namespace H.Generators;

internal static class SourceGenerationHelper
{
    public static string GenerateEvent(ClassData @class, EventData @event)
    {
        var sender = @event.IsStatic
            ? "sender"
            : "this";
        
        return @$" 
#nullable enable

namespace {@class.Namespace}
{{
    {@class.Modifiers} partial class {@class.Name}
    {{
{GenerateXmlDocumentationFrom(@event.XmlDocumentation, @event)}
        public{(@event.IsStatic ? " static" : "")} event {GenerateEventHandlerType(@class, @event)}? {@event.Name};

        /// <summary>
        /// A helper method to raise the {@event.Name} event.
        /// </summary>
        {GenerateOnEventModifier(@class, @event)} {GenerateEventArgsType(@class, @event)} On{@event.Name}({GenerateOnEventParameters(@class, @event)})
        {{
            {GenerateArgs(@class, @event)}
            {@event.Name}?.Invoke({sender}, args);

            return args;
        }}
{(!@event.Types.Any() || @event.IsEventArgs ? " " : $@"
        /// <summary>
        /// A helper method to raise the {@event.Name} event.
        /// </summary>
        {GenerateOnEventModifier(@class, @event)} {GenerateEventArgsType(@class, @event)} On{@event.Name}(
{GenerateOnEventOverloadParameters(@event)})
        {{
            var args = new {GenerateEventArgsType(@class, @event)}({string.Join(", ", @event.Types.Select(static type => type.ParameterName))});
            {@event.Name}?.Invoke({sender}, args);

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
        /// <summary>
        /// 
        /// </summary>
        public class {@event.Name}EventArgs : global::System.EventArgs
        {{
{@event.Types.Select(static type => @$" 
            /// <summary>
            /// 
            /// </summary>
            public {GenerateType(type)} {type.PropertyName} {{ get; }}

".RemoveBlankLinesWhereOnlyWhitespaces()).Inject()}

            /// <summary>
            /// 
            /// </summary>
            public {@event.Name}EventArgs({string.Join(", ", @event.Types.Select(static type => $"{GenerateType(type)} {type.ParameterName}"))})
            {{
{@event.Types.Select(static type => @$" 
                {type.PropertyName} = {type.ParameterName};
".RemoveBlankLinesWhereOnlyWhitespaces()).Inject()}
            }}

            /// <summary>
            /// 
            /// </summary>
            public void Deconstruct({string.Join(", ", @event.Types.Select(static type => $"out {GenerateType(type)} {type.ParameterName}"))})
            {{
{@event.Types.Select(static type => @$" 
                {type.ParameterName} = {type.PropertyName};
".RemoveBlankLinesWhereOnlyWhitespaces()).Inject()}
            }}

            /// <summary>
            /// 
            /// </summary>
            public override string ToString()
            {{
                return $""({string.Join(", ", @event.Types.Select(static type => $"{type.PropertyName}={{{type.PropertyName}}}"))})"";
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

    private static string GenerateOnEventModifier(ClassData @class, EventData @event)
    {
        var modifiers = string.Empty;
        if (@event.IsStatic)
        {
            modifiers += "internal static";
        }
        else if (@class.IsSealed)
        {
            modifiers += "private";
        }
        else
        {
            modifiers += "protected virtual";
        }
        
        return modifiers;
    }

    private static string GenerateOnEventParameters(ClassData @class, EventData @event)
    {
        var args = new List<string>(2);
        if (@event.IsStatic)
        {
            args.Add("object? sender");
        }
        if (@event.Types.Any())
        {
            args.Add($"{GenerateEventArgsType(@class, @event)} args");
        }

        return string.Join(", ", args);
    }

    private static string GenerateOnEventOverloadParameters(EventData @event)
    {
        var args = new List<string>(1 + @event.Types.Length);
        if (@event.IsStatic)
        {
            args.Add("object? sender");
        }
        args.AddRange(@event.Types.Select(type => $"{GenerateType(type)} {type.ParameterName}"));

        return args.Select(static arg => @$" 
            {arg},
".RemoveBlankLinesWhereOnlyWhitespaces()).Inject().Trim(',');
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
        if (@event.Types.Any() &&
            !@event.IsEventArgs)
        {
            return $"{GenerateType($"{@class.Namespace}.{@class.Name}.{@event.Name}EventArgs", false)}";
        }
        if (@event.IsEventArgs)
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