using System;
using System.Collections.Generic;
using System.Text;

namespace VSlices.Infrastructure.Traits.InMemoryQueueIO;

public abstract class InMemoryQueueConfiguration
{
    public sealed class Bounded : InMemoryQueueConfiguration
    {
        public int Capacity { get; }

        public Bounded(int capacity)
        {
            if (capacity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be greater than zero.");
            }

            Capacity = capacity;
        }
    }

    public sealed class Unbounded : InMemoryQueueConfiguration;

    public static Bounded CreateBounded(uint capability = 20) =>
        new((int)capability);

    public static Unbounded CreateUnbounded() =>
        new();
}

public interface InMemoryQueueIO
{
    IO<InMemoryQueueConfiguration> Configuration { get; }

}

