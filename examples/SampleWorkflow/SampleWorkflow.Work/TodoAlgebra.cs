using SampleWorkflow.Spaces;
using VSlices.Work;

namespace SampleWorkflow.Work;

public interface TodoIdSource
{
    IO<TodoId> Next();
}

public sealed record TodoAlgebra(
    PointReader<Todo, TodoId> Reader,
    PointWriter<Todo> Writer,
    PointRemover<Todo, TodoId> Remover,
    TodoIdSource Ids);
