using LanguageExt;
using SampleWorkflow.Spaces;
using VSlices.Monads;
using VSlices.Work;
using static LanguageExt.Prelude;

namespace SampleWorkflow.Work;

public sealed class DeleteTodo :
    Feature<DeleteTodo, TodoAlgebra, DeleteTodo.Request, DeleteTodo.Response>
{
    public sealed record Request(TodoId Id);
    public sealed record Response(Option<Todo> Todo);

    public static Flow<TodoAlgebra, Request, Response> Get() =>
        new((algebra, request) =>
            algebra.Reader.Read(request.Id)
                .Bind(current =>
                    current.Match(
                        Some: point =>
                            algebra.Remover.Remove(request.Id)
                                .Map(_ => new Response(Some(point))),
                        None: static () =>
                            IO.pure(new Response(Option<Todo>.None)))));
}
