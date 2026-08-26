using System.Security.Claims;
using LanguageExt;
using VSlices.Domain.Traits;
using VSlices.Products;
using VSlices.Services;

namespace Shared.Domain.Products;

public sealed class RoleSet(
    UserType userType,
    Seq<UserAccessRole> accesses,
    Seq<AppSpecificRole> specifics)
    : DomainType<RoleSet, RoleSet.Repr>
{
    public sealed record Repr(
        vRole.Repr UserType,
        Seq<vRole.Repr> Accesses,
        Seq<vRole.Repr> Specifics);

    public UserType UserType { get; } = userType;

    public Seq<UserAccessRole> Accesses { get; } = accesses;

    public Seq<AppSpecificRole> Specifics { get; } = specifics;

    public Seq<vClaim> Claims =>
        Accesses.Bind(r => r.Claims)
            .Concat(Specifics.Bind(r => r.Claims))
            .Distinct();

    public Repr To() =>
        new(
            UserType.To(),
            Accesses.Map(r => r.To()),
            Specifics.Map(r => r.To()));
}
