namespace VSlices.Domain.Traits;

public interface EventYielderIO
{
    IO<DomainEvent> Yield();

}
