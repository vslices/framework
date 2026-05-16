namespace VSlices.Errors;

/// <summary>
/// Represents a feature error that occurs during application execution.
/// </summary>
/// <param name="Message">The message describing the error.</param>
/// <param name="Code">The error code associated with the feature error.</param>
public abstract record FeatureError(string Message, int Code)
    : Expected(Message, Code, None);

/// <summary>
/// Represents an error indicating that authentication is required to access the requested resource.
/// </summary>
/// <param name="Message">The message describing the authentication requirement.</param>
public sealed record NotAuthenticated(
    string Message = "Authentication is required.")
    : FeatureError(Message, 401);

/// <summary>
/// Represents an error indicating that the user is not authorized to perform the requested action.
/// </summary>
/// <param name="Message">The message describing the authorization failure.</param>
public sealed record NotAuthorized(
    string Message = "You are not authorized to perform this action.")
    : FeatureError(Message, 403);

/// <summary>
/// Represents an error indicating that the requested resource could not be found.
/// </summary>
/// <param name="Message">The message describing the resource not found error.</param>
public sealed record ResourceNotFound(
    string Message = "The requested resource was not found.")
    : FeatureError(Message, 404);

/// <summary>
/// Represents an error indicating a conflict state in the application.
/// </summary>
/// <param name="Issues">A sequence of issues describing the conflict state.</param>
/// <param name="Message">The message providing details about the conflict state.</param>
public sealed record ConflictedState(
    Seq<ConflictStateIssue> Issues,
    string Message)
    : FeatureError(Message, 409);

/// <summary>
/// Represents an error indicating that the requested resource is no longer available.
/// </summary>
/// <param name="Message">The message describing the resource unavailability.</param>
public sealed record Gone(
    string Message = "The requested resource is no longer available.")
    : FeatureError(Message, 410);

/// <summary>
/// Represents a validation failure that contains one or more validation issues.
/// </summary>
/// <param name="Issues">The sequence of validation issues that caused the failure.</param>
/// <param name="Message">A human-readable message describing the validation failure.</param>
public sealed record ValidationUnfulfilled(
    Seq<ValidationIssue> Issues,
    string Message = "Validation failed.")
    : FeatureError(Message, 422);

/// <summary>
/// Represents an error indicating that access to the requested resource is blocked.
/// </summary>
/// <param name="Message">The message describing the access block error.</param>
public sealed record AccessBlocked(
    string Message = "The requested resource is blocked.")
    : FeatureError(Message, 423);
