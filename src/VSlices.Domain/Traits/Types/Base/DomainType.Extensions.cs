using VSlices.Domain.Traits;

namespace VSlices;

/// <summary>
/// Provides deconstruction helpers for domain types backed by tuple-like representations.
/// </summary>
public static partial class DomainTypeExtensions
{
    extension<REPR>(REPR self)
        where REPR : DomainRepresent<REPR>
    {
        /// <summary>
        /// Deconstructs the domain value into it underlying component.
        /// </summary>
        public REPR To() => self.To();
    }
}
