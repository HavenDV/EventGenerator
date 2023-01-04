// ReSharper disable RedundantNameQualifier
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global
#nullable enable

namespace EventGenerator;

/// <summary>
/// Generates an event.
/// </summary>
[global::System.AttributeUsage(global::System.AttributeTargets.Class, AllowMultiple = true)]
[global::System.Diagnostics.Conditional("EVENTGENERATOR_ATTRIBUTES")]
public class EventAttribute : global::System.Attribute
{
    /// <summary>
    /// Name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Types of event handler.
    /// </summary>
    public global::System.Type[] Types { get; }

    /// <summary>
    /// Names of generated EventArgs properties.
    /// </summary>
    public string[] PropertyNames { get; set; }

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
    /// <param name="types"></param>
    /// <exception cref="global::System.ArgumentNullException"></exception>
    public EventAttribute(
        string name,
        params global::System.Type[] types)
    {
        Name = name ?? throw new global::System.ArgumentNullException(nameof(name));
        Types = types;
        // Disabled because this code is delivered to the final project directly and it may be lower than .Net Framework 4.6
        // ReSharper disable once UseArrayEmptyMethod
#pragma warning disable CA1825
        PropertyNames = new string[0];
#pragma warning restore CA1825
    }
}

/// <summary>
/// Generates an event.
/// </summary>
/// <typeparam name="T">Type of event handler.</typeparam>
[global::System.AttributeUsage(global::System.AttributeTargets.Class, AllowMultiple = true)]
[global::System.Diagnostics.Conditional("EVENTGENERATOR_ATTRIBUTES")]
public sealed class EventAttribute<T> : EventAttribute
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <exception cref="global::System.ArgumentNullException"></exception>
    public EventAttribute(
        string name) : base(name, typeof(T))
    {
    }
}

/// <summary>
/// Generates an event.
/// </summary>
/// <typeparam name="T1">Type of event handler.</typeparam>
/// <typeparam name="T2">Type of event handler.</typeparam>
[global::System.AttributeUsage(global::System.AttributeTargets.Class, AllowMultiple = true)]
[global::System.Diagnostics.Conditional("EVENTGENERATOR_ATTRIBUTES")]
public sealed class EventAttribute<T1, T2> : EventAttribute
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <exception cref="global::System.ArgumentNullException"></exception>
    public EventAttribute(
        string name) : base(name, typeof(T1), typeof(T2))
    {
    }
}

/// <summary>
/// Generates an event.
/// </summary>
/// <typeparam name="T1">Type of event handler.</typeparam>
/// <typeparam name="T2">Type of event handler.</typeparam>
/// <typeparam name="T3">Type of event handler.</typeparam>
[global::System.AttributeUsage(global::System.AttributeTargets.Class, AllowMultiple = true)]
[global::System.Diagnostics.Conditional("EVENTGENERATOR_ATTRIBUTES")]
public sealed class EventAttribute<T1, T2, T3> : EventAttribute
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <exception cref="global::System.ArgumentNullException"></exception>
    public EventAttribute(
        string name) : base(name, typeof(T1), typeof(T2), typeof(T3))
    {
    }
}

/// <summary>
/// Generates an event.
/// </summary>
/// <typeparam name="T1">Type of event handler.</typeparam>
/// <typeparam name="T2">Type of event handler.</typeparam>
/// <typeparam name="T3">Type of event handler.</typeparam>
/// <typeparam name="T4">Type of event handler.</typeparam>
[global::System.AttributeUsage(global::System.AttributeTargets.Class, AllowMultiple = true)]
[global::System.Diagnostics.Conditional("EVENTGENERATOR_ATTRIBUTES")]
public sealed class EventAttribute<T1, T2, T3, T4> : EventAttribute
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <exception cref="global::System.ArgumentNullException"></exception>
    public EventAttribute(
        string name) : base(name, typeof(T1), typeof(T2), typeof(T3), typeof(T4))
    {
    }
}

/// <summary>
/// Generates an event.
/// </summary>
/// <typeparam name="T1">Type of event handler.</typeparam>
/// <typeparam name="T2">Type of event handler.</typeparam>
/// <typeparam name="T3">Type of event handler.</typeparam>
/// <typeparam name="T4">Type of event handler.</typeparam>
/// <typeparam name="T5">Type of event handler.</typeparam>
[global::System.AttributeUsage(global::System.AttributeTargets.Class, AllowMultiple = true)]
[global::System.Diagnostics.Conditional("EVENTGENERATOR_ATTRIBUTES")]
public sealed class EventAttribute<T1, T2, T3, T4, T5> : EventAttribute
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <exception cref="global::System.ArgumentNullException"></exception>
    public EventAttribute(
        string name) : base(name, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5))
    {
    }
}

/// <summary>
/// Generates an event.
/// </summary>
/// <typeparam name="T1">Type of event handler.</typeparam>
/// <typeparam name="T2">Type of event handler.</typeparam>
/// <typeparam name="T3">Type of event handler.</typeparam>
/// <typeparam name="T4">Type of event handler.</typeparam>
/// <typeparam name="T5">Type of event handler.</typeparam>
/// <typeparam name="T6">Type of event handler.</typeparam>
[global::System.AttributeUsage(global::System.AttributeTargets.Class, AllowMultiple = true)]
[global::System.Diagnostics.Conditional("EVENTGENERATOR_ATTRIBUTES")]
public sealed class EventAttribute<T1, T2, T3, T4, T5, T6> : EventAttribute
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <exception cref="global::System.ArgumentNullException"></exception>
    public EventAttribute(
        string name) : base(name, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6))
    {
    }
}

/// <summary>
/// Generates an event.
/// </summary>
/// <typeparam name="T1">Type of event handler.</typeparam>
/// <typeparam name="T2">Type of event handler.</typeparam>
/// <typeparam name="T3">Type of event handler.</typeparam>
/// <typeparam name="T4">Type of event handler.</typeparam>
/// <typeparam name="T5">Type of event handler.</typeparam>
/// <typeparam name="T6">Type of event handler.</typeparam>
/// <typeparam name="T7">Type of event handler.</typeparam>
[global::System.AttributeUsage(global::System.AttributeTargets.Class, AllowMultiple = true)]
[global::System.Diagnostics.Conditional("EVENTGENERATOR_ATTRIBUTES")]
public sealed class EventAttribute<T1, T2, T3, T4, T5, T6, T7> : EventAttribute
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <exception cref="global::System.ArgumentNullException"></exception>
    public EventAttribute(
        string name) : base(name, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7))
    {
    }
}

/// <summary>
/// Generates an event.
/// </summary>
/// <typeparam name="T1">Type of event handler.</typeparam>
/// <typeparam name="T2">Type of event handler.</typeparam>
/// <typeparam name="T3">Type of event handler.</typeparam>
/// <typeparam name="T4">Type of event handler.</typeparam>
/// <typeparam name="T5">Type of event handler.</typeparam>
/// <typeparam name="T6">Type of event handler.</typeparam>
/// <typeparam name="T7">Type of event handler.</typeparam>
/// <typeparam name="T8">Type of event handler.</typeparam>
[global::System.AttributeUsage(global::System.AttributeTargets.Class, AllowMultiple = true)]
[global::System.Diagnostics.Conditional("EVENTGENERATOR_ATTRIBUTES")]
public sealed class EventAttribute<T1, T2, T3, T4, T5, T6, T7, T8> : EventAttribute
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <exception cref="global::System.ArgumentNullException"></exception>
    public EventAttribute(
        string name) : base(name, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8))
    {
    }
}

/// <summary>
/// Generates an event.
/// </summary>
/// <typeparam name="T1">Type of event handler.</typeparam>
/// <typeparam name="T2">Type of event handler.</typeparam>
/// <typeparam name="T3">Type of event handler.</typeparam>
/// <typeparam name="T4">Type of event handler.</typeparam>
/// <typeparam name="T5">Type of event handler.</typeparam>
/// <typeparam name="T6">Type of event handler.</typeparam>
/// <typeparam name="T7">Type of event handler.</typeparam>
/// <typeparam name="T8">Type of event handler.</typeparam>
/// <typeparam name="T9">Type of event handler.</typeparam>
[global::System.AttributeUsage(global::System.AttributeTargets.Class, AllowMultiple = true)]
[global::System.Diagnostics.Conditional("EVENTGENERATOR_ATTRIBUTES")]
public sealed class EventAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9> : EventAttribute
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <exception cref="global::System.ArgumentNullException"></exception>
    public EventAttribute(
        string name) : base(name, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9))
    {
    }
}
