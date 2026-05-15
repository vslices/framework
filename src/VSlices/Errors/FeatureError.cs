using System;
using System.Collections.Generic;
using System.Text;

namespace VSlices.Core.Errors;

public abstract record FeatureError(string Message, int Code)
    : Expected(Message, Code, None);

public sealed record NotAuthenticated(
    string Message = "Authentication is required.")
    : FeatureError(Message, 401);

public sealed record NotAuthorized(
    string Message = "You are not authorized to perform this action.")
    : FeatureError(Message, 403);

public sealed record ResourceNotFound(
    string Message = "The requested resource was not found.")
    : FeatureError(Message, 404);

public sealed record ConflictedState(
    Seq<ConflictStateIssue> Issues,
    string Message)
    : FeatureError(Message, 409);

public sealed record Gone(
    string Message = "The requested resource is no longer available.")
    : FeatureError(Message, 410);

public sealed record ValidationUnfulliled(
    Seq<ValidationIssue> Issues,
    string Message = "Validation failed.")
    : FeatureError(Message, 422);

public sealed record AccessBlocked(
    string Message = "The requested resource is blocked.")
    : FeatureError(Message, 423);
