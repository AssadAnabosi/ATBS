namespace ATBS.Models;

public sealed class FieldConstraint
{
    public required string FieldName { get; init; }
    public required string Type { get; init; }
    public required List<string> Constraints { get; init; }

    public override string ToString() =>
        $"{FieldName}: Type = {Type}; Constraint = {string.Join(", ", Constraints)}";
}