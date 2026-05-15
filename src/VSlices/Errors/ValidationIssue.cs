namespace VSlices.Core.Errors;

public sealed record ValidationIssue(string Detail, string PropertyPath);
