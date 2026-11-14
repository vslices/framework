using VSlices.Spaces.Constants;
using LanguageExt;
using LanguageExt.Common;
using VSlices.Spaces;
using VSlices.Spaces.Traits;

namespace VSlices.Spaces.Definitions;

/// <summary>
/// The observation of an authentication mechanism. Read succeeds when a valid
/// context was observed, not necessarily when credentials were authenticated.
/// </summary>
public abstract record AuthenticationContext
{
    protected AuthenticationContext(AuthenticationMethod method) => Method = method;

    public AuthenticationMethod Method { get; }

    public abstract Identity Identity { get; }

    public sealed record Authenticated :
        AuthenticationContext,
        DiscreteSpace<Authenticated>,
        Transformable<Authenticated.Input, Authenticated>
    {
        public readonly record struct Input(AuthenticationMethod Method, Identity Identity);

        private Authenticated(AuthenticationMethod method, Identity identity)
            : base(method) => Identity = identity;

        public override Identity Identity { get; }

        public static Fin<Authenticated> Transform(Input input) =>
            input.Method is null || input.Identity is null ||
            input.Identity is UnauthenticatedIdentity
                ? Fin.Fail<Authenticated>(Error.New(
                    "Authenticated context requires method and recognized identity."))
                : new Authenticated(input.Method, input.Identity);
    }

    public sealed record Unauthenticated :
        AuthenticationContext,
        DiscreteSpace<Unauthenticated>,
        Transformable<Unauthenticated.Input, Unauthenticated>
    {
        public readonly record struct Input(AuthenticationMethod Method);

        private Unauthenticated(AuthenticationMethod method) : base(method) { }

        public override Identity Identity => UnauthenticatedIdentity.Value;

        public static Fin<Unauthenticated> Transform(Input input) =>
            input.Method is null
                ? Fin.Fail<Unauthenticated>(Error.New("Authentication method is required."))
                : new Unauthenticated(input.Method);
    }

    public sealed record Rejected :
        AuthenticationContext,
        DiscreteSpace<Rejected>,
        Transformable<Rejected.Input, Rejected>
    {
        public readonly record struct Input(AuthenticationMethod Method);

        private Rejected(AuthenticationMethod method) : base(method) { }

        public override Identity Identity => UnauthenticatedIdentity.Value;

        public static Fin<Rejected> Transform(Input input) =>
            input.Method is null
                ? Fin.Fail<Rejected>(Error.New("Authentication method is required."))
                : new Rejected(input.Method);
    }
}
