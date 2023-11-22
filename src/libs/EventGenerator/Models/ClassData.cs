namespace H.Generators;

public readonly record struct ClassData(
    string Namespace,
    string Name,
    string Type,
    string Modifiers,
    bool IsSealed);
