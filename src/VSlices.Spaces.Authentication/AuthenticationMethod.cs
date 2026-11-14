using LanguageExt;
using LanguageExt.Common;
using VSlices.Spaces;
using VSlices.Spaces.Traits;

namespace VSlices.Spaces.Authentication;

/// <summary>
/// Complete set of authentication methods currently recognized by VSlices.
/// A method is not a scheme name, cookie name or proof of successful authentication.
/// </summary>
public sealed record AuthenticationMethod :
    MaintainedSpace<AuthenticationMethod>,
    Transformable<AuthenticationMethod.Input, AuthenticationMethod>
{
    public readonly record struct Input(string Name);

    private AuthenticationMethod(string name) => Name = name;

    public string Name { get; }

    public static AuthenticationMethod None { get; } = new("None");
    public static AuthenticationMethod Cookie { get; } = new("Cookie");

    public static Seq<AuthenticationMethod> All =>
        LanguageExt.Prelude.Seq(None, Cookie);

    public static Fin<AuthenticationMethod> Transform(Input input) =>
        input.Name switch
        {
            "None" => None,
            "Cookie" => Cookie,
            _ => Fin.Fail<AuthenticationMethod>(
                Error.New("Unrecognized authentication method."))
        };
}
