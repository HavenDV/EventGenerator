namespace H.Generators;

public readonly record struct EventData(
    string Name,
    string? Type,
    string? Description,
    string? XmlDocumentation);
