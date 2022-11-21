namespace H.Generators;

public readonly record struct EventData(
    string Name,
    TypeData[] Types,
    string? Description,
    string? XmlDocumentation);
