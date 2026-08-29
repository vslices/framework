namespace VSlices.Errors;

/// <summary>
/// Represents an issue that describes a conflict state in the application.
/// </summary>
/// <param name="Detail">A detailed description of the conflict state issue.</param>
public sealed record ConflictStateIssue(string Detail);