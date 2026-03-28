namespace VSlices.Infrastructure.Traits.InMemoryQueueIO;

public record InMemoryQueue<M, RT>
    where M : MonadIO<M>
    where RT : Has<M, InMemoryQueueIO>
{
    static K<M, InMemoryQueueIO> inMemoryQueueIO => Has<M, RT, InMemoryQueueIO>.ask;

    public static K<M, InMemoryQueueConfiguration> getConfiguration() =>
        inMemoryQueueIO.Bind(io => io.Configuration);

}

public record InMemoryQueue<RT>
    where RT : Has<Eff<RT>, InMemoryQueueIO>
{
    public static Eff<RT, InMemoryQueueConfiguration> getConfiguration() =>
        InMemoryQueue<Eff<RT>, RT>.getConfiguration().As();
}
