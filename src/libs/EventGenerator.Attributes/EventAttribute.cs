namespace EventGenerator;

/// <summary>
/// Generates an event.
/// </summary>
[global::System.AttributeUsage(global::System.AttributeTargets.Class, AllowMultiple = true)]
[global::System.Diagnostics.Conditional("EVENTGENERATOR_ATTRIBUTES")]
public sealed class EventAttribute : global::System.Attribute
{
    /// <summary>
    /// Name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Type of event handler.
    /// </summary>
    public global::System.Type? Type { get; }

    /// <summary>
    /// Description of this dependency property. <br/>
    /// This will also be used in the xml documentation if not explicitly specified. <br/>
    /// Default - <see langword="null"/>.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// The event xml documentation. <br/>
    /// Default - "&lt;summary&gt;&lt;/summary&gt;".
    /// </summary>
    public string XmlDocumentation { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="type"></param>
    /// <exception cref="global::System.ArgumentNullException"></exception>
    public EventAttribute(
        string name,
        global::System.Type? type = null)
    {
        Name = name ?? throw new global::System.ArgumentNullException(nameof(name));
        Type = type;
    }
}

/// <summary>
/// Generates an event.
/// </summary>
/// <typeparam name="T">Type of event handler.</typeparam>
[global::System.AttributeUsage(global::System.AttributeTargets.Class, AllowMultiple = true)]
[global::System.Diagnostics.Conditional("EVENTGENERATOR_ATTRIBUTES")]
public sealed class EventAttribute<T> : global::System.Attribute
{
    /// <summary>
    /// Name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Type of event handler.
    /// </summary>
    public global::System.Type Type { get; }

    /// <summary>
    /// Description of this dependency property. <br/>
    /// This will also be used in the xml documentation if not explicitly specified. <br/>
    /// Default - <see langword="null"/>.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// The event xml documentation. <br/>
    /// Default - "&lt;summary&gt;&lt;/summary&gt;".
    /// </summary>
    public string XmlDocumentation { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <exception cref="global::System.ArgumentNullException"></exception>
    public EventAttribute(
        string name)
    {
        Name = name ?? throw new global::System.ArgumentNullException(nameof(name));
        Type = typeof(T);
    }
}