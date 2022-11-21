namespace H.Generators;

public readonly record struct EventData(
    string Name,
    TypeData[] Types,
    string? Description,
    string? XmlDocumentation)
{
    public bool IsEventArgs =>
        Types.Length == 1 &&
        Types[0].FullName.Contains("EventArgs");
}
