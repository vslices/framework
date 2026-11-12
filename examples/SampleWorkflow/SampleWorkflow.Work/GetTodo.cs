using System.Runtime.CompilerServices;
using LanguageExt;
using SampleWorkflow.Spaces;
using VSlices.Monads;
using VSlices.Work;

namespace SampleWorkflow.Work;

public sealed class GetTodo :
    Feature<GetTodo, TodoAlgebra, GetTodo.Request, GetTodo.Response>
{
    public sealed record Request(TodoId Id);
    public sealed record Response
    {
        private Response(Option<Todo> todo) => Todo = todo;
        public Option<Todo> Todo { get; }
    }

    [UnsafeAccessor(UnsafeAccessorKind.Constructor)]
    private static extern Response NewResponse(Option<Todo> todo);

    public static Eff<TodoAlgebra, Response> Get(Request request) =>
        Eff<TodoAlgebra, Response>.LiftIO(algebra =>
            algebra.Reader.ReadOrDefault(request.Id).Run().As()
                .Map(todo => NewResponse(todo)));
}
