using VSlices.Domain.Environments.EventBuffer;
using VSlices.Domain.Traits;
using VSlices.Infrastructure.Services;

namespace VSlices.Infrastructure.Implementations;

public sealed class InMemoryEventBufferIO(InMemoryEventQueue queue) : EventBufferIO
{
    private readonly InMemoryEventQueue _queue = queue;
    private readonly object _sync = new();

    // Scoped: tracking solo para la transacción/request actual
    private LanguageExt.HashSet<UntypedAggregateRoot> _tracked = [];

    public IO<Unit> Track(UntypedAggregateRoot root) =>
        liftIO(() =>
        {
            lock (_sync)
            {
                _tracked = _tracked.Add(root);
            }

            return unit.AsTask();
        });

    public IO<Unit> Commit() =>
        liftIO(() =>
        {
            List<UntypedAggregateRoot> snapshot;

            lock (_sync)
            {
                snapshot = [.. _tracked];
                _tracked = _tracked.Clear();
            }

            foreach (var root in snapshot)
            {
                var events = root.DequeueEvents();

                foreach (var @event in events)
                {
                    var buffered = new BufferedDomainEvent(
                        Event: @event,
                        AggregateType: root.GetType(),
                        EnqueuedAtUtc: DateTimeOffset.UtcNow);

                    if (!_queue.Writer.TryWrite(buffered))
                    {
                        throw new InvalidOperationException(
                            "No fue posible encolar un DomainEvent en InMemoryEventQueue.");
                    }
                }
            }

            return unit.AsTask();
        });
}
