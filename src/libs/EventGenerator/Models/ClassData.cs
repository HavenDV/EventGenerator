namespace H.Generators;

public readonly record struct ClassData(
    string Namespace,
    string Name,
    string Modifiers,
    bool IsSealed,
    IReadOnlyCollection<EventData> Events);
