namespace H.Generators;

public readonly record struct TypeData(
    string FullName,
    bool IsSpecial,
    string PropertyName,
    string ParameterName);
