using System.Collections.Concurrent;
using VSlices.Domain.Traits;

namespace VSlices.Services;

public sealed class ServiceClaim : DomainType<ServiceClaim, ServiceClaim.Repr>
{
    private sealed record Registration(
        ServiceClaim Claim,
        Type Owner);

    private static readonly ConcurrentDictionary<string, Registration> Registry =
        new(StringComparer.Ordinal);

    public readonly record struct Repr(
        string UniqueName,
        string Description);

    private ServiceClaim(
        string uniqueName,
        string description) =>
        (UniqueName, Description) =
        (uniqueName, description);

    public string UniqueName { get; }

    public string Description { get; }

    public static ServiceClaim New<OWNER>(
        string uniqueName,
        string description)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(uniqueName);
        ArgumentException.ThrowIfNullOrWhiteSpace(description);

        var owner = typeof(OWNER);
        var candidate = new Registration(
            new ServiceClaim(uniqueName, description),
            owner);

        var registered = Registry.GetOrAdd(
            uniqueName,
            candidate);

        if (registered.Owner != owner)
        {
            throw new NotSupportedException(
                $"ServiceClaim '{uniqueName}' is already registered by " +
                $"'{registered.Owner.FullName}' and cannot also be registered by " +
                $"'{owner.FullName}'.");
        }

        return registered.Claim;
    }

    public Repr To() =>
        new(
            UniqueName,
            Description);

    public override string ToString() =>
        UniqueName;

    public override bool Equals(object? obj) =>
        obj is ServiceClaim other &&
        UniqueName.Equals(
            other.UniqueName,
            StringComparison.Ordinal);

    public override int GetHashCode() =>
        StringComparer.Ordinal.GetHashCode(UniqueName);
}
