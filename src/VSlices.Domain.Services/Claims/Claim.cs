using VSlices.Domain.Traits;

namespace VSlices.Services;

public abstract class ServiceClaim : DomainType<ServiceClaim, ServiceClaim.Repr>
{
    public readonly record struct Repr(string Service, string Capability);

    public abstract string Service { get; }

    public abstract string Capability { get; }

    public Repr To() =>
        new(Service, Capability);

    public override string ToString() =>
        $"{Service}.{Capability}";

    public override bool Equals(object? obj) =>
        obj is ServiceClaim other &&
        Service.Equals(other.Service, StringComparison.Ordinal) &&
        Capability.Equals(other.Capability, StringComparison.Ordinal);

    public override int GetHashCode() =>
        HashCode.Combine(
            StringComparer.Ordinal.GetHashCode(Service),
            StringComparer.Ordinal.GetHashCode(Capability));
}
