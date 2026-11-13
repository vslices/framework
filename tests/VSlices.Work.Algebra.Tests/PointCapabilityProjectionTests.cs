using VSlices.Work.Algebra;
using VSlices.Work;
using Xunit;

namespace VSlices.Work.Tests;

public sealed class PointCapabilityProjectionTests
{
    [Fact]
    public void Capability_projections_expose_the_exact_point_atoms()
    {
        Assert.Equal(typeof(PointReader<Account, AccountId>),
            typeof(HasPointReader<Account, AccountId>)
                .GetProperty(nameof(HasPointReader<Account, AccountId>.Reader))!.PropertyType);
        Assert.Equal(typeof(PointWriter<Account>),
            typeof(HasPointWriter<Account>)
                .GetProperty(nameof(HasPointWriter<Account>.Writer))!.PropertyType);
        Assert.Equal(typeof(PointRemover<Account, AccountId>),
            typeof(HasPointRemover<Account, AccountId>)
                .GetProperty(nameof(HasPointRemover<Account, AccountId>.Remover))!.PropertyType);
    }

    [Fact]
    public void Feature_can_use_reader_without_access_to_writer_or_remover()
    {
        var grounding = new InMemoryAccountGrounding([]);
        ReaderOnlyAlgebra runtime = new ComposedAlgebra(grounding, grounding);

        Assert.Same(grounding, runtime.Reader);
        Assert.DoesNotContain(typeof(HasPointWriter<Account>),
            typeof(ReaderOnlyAlgebra).GetInterfaces());
        Assert.DoesNotContain(typeof(HasPointRemover<Account, AccountId>),
            typeof(ReaderOnlyAlgebra).GetInterfaces());
    }

    private interface ReaderOnlyAlgebra : HasPointReader<Account, AccountId> { }

    private sealed record ComposedAlgebra(
        PointReader<Account, AccountId> Reader,
        PointWriter<Account> Writer) :
        ReaderOnlyAlgebra,
        HasPointWriter<Account>;
}
