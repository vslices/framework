using LanguageExt;
using System.Security.Claims;
using VSlices.Products;
using VSlices.Services;

namespace Shared.Domain.Products;

public sealed class UserType : vRole
{
    private UserType(string name) =>
        Name = name;

    public override string Name { get; }

    public override Seq<vClaim> Claims { get; } = [];

    public static UserType Admin { get; } =
        new("Admin");

    public static UserType Internal { get; } =
        new("Internal");

    public static UserType External { get; } =
        new("External");

    public static Seq<UserType> All { get; } =
    [
        Admin,
        Internal,
        External
    ];
}
