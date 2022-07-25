using System.Diagnostics;

namespace EventGenerator;

/// <summary>
/// Generates an event.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
[Conditional("EVENTGENERATOR_ATTRIBUTES")]
public sealed class EventAttribute : Attribute
{
    /// <summary>
    /// Name.
    /// </summary>
	public string Name { get; }

    /// <summary>
    /// Type of event handler.
    /// </summary>
    public Type? Type { get; }

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
    /// <exception cref="ArgumentNullException"></exception>
    public EventAttribute(
        string name,
        Type? type = null)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Type = type;
    }
}

/// <summary>
/// Generates an event.
/// </summary>
/// <typeparam name="T">Type of event handler.</typeparam>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
[Conditional("EVENTGENERATOR_ATTRIBUTES")]
public sealed class EventAttribute<T> : Attribute
{
    /// <summary>
    /// Name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Type of event handler.
    /// </summary>
    public Type Type { get; }

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
    /// <exception cref="ArgumentNullException"></exception>
    public EventAttribute(
        string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Type = typeof(T);
    }
}