namespace VSlices.Errors;

/// <summary>
/// Represents a validation issue encountered during processing.
/// </summary>
/// <param name="Detail">A detailed description of the validation issue.</param>
/// <param name="PropertyPath">The path to the property associated with the validation issue.</param>
public sealed record ValidationIssue(string Detail, string PropertyPath);
