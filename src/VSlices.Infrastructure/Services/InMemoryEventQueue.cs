using System.Threading.Channels;
using VSlices.Domain.Traits;
using VSlices.Infrastructure.Traits.InMemoryQueueIO;

namespace VSlices.Infrastructure.Services;

public sealed record BufferedDomainEvent(
    DomainEvent Event,
    Type AggregateType,
    DateTimeOffset EnqueuedAtUtc);

public sealed class InMemoryEventQueue
{
    private readonly Channel<BufferedDomainEvent> _channel;

    private InMemoryEventQueue(InMemoryQueueConfiguration config)
    {
        _channel = config switch
        {
            InMemoryQueueConfiguration.Unbounded => 
                Channel.CreateUnbounded<BufferedDomainEvent>(new UnboundedChannelOptions
                {
                    SingleReader = false,
                    SingleWriter = false
                }),
            InMemoryQueueConfiguration.Bounded conf => 
                Channel.CreateBounded<BufferedDomainEvent>(new BoundedChannelOptions(conf.Capacity)
                {
                    SingleReader = false,
                    SingleWriter = false,
                    FullMode = BoundedChannelFullMode.Wait
                }),
            _ => throw new InvalidOperationException("Unsupported InMemoryQueueConfiguration type.")
        };
    }

    public ChannelReader<BufferedDomainEvent> Reader =>
        _channel.Reader;

    public ChannelWriter<BufferedDomainEvent> Writer =>
        _channel.Writer;

    public static K<M, InMemoryEventQueue> Create<M, RT>()
        where M : MonadIO<M>
        where RT : Has<M, InMemoryQueueIO> =>
        from config in InMemoryQueue<M, RT>.getConfiguration()
        select new InMemoryEventQueue(config);

}
