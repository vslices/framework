using LanguageExt;
using LanguageExt.Common;
using SampleWorkflow.Spaces;
using VSlices.Space;
using VSlices.Work;
using static LanguageExt.Prelude;
using SampleWorkflow.Work.Algebras;

namespace SampleWorkflow.Work;

public sealed class UpdateTodoOptimized :
    Feature<UpdateTodoOptimized,
        TodoAlgebraOptimized,
        UpdateTodoOptimized.Request,
        UpdateTodoOptimized.Response>
{
    public sealed record Request(TodoId Id, TodoDetail Detail, bool Completed);
    
    public sealed record Response(Either<Error, Option<Todo>> Todo);

    public static Free<TodoAlgebraOptimized, Response> Get(Request request) =>
        from current in PointReader.read<TodoAlgebraOptimized, Todo, TodoId>(request.Id)
        from response in current.Match(
            Some: todo =>
                todo.Update(state => state with
                    {
                        Detail = request.Detail,
                        Completed = request.Completed
                    })
                    .Match(
                        Succ: updated =>
                            from _ in PointWriter.write<TodoAlgebraOptimized, Todo>(updated)
                            select new Response(
                                Either.Right<Error, Option<Todo>>(Some(updated))),
                        Fail: error =>
                            Free.pure<TodoAlgebraOptimized, Response>(
                                new Response(
                                    Either.Left<Error, Option<Todo>>(error)))),
            None: static () =>
                Free.pure<TodoAlgebraOptimized, Response>(
                    new Response(
                        Either.Right<Error, Option<Todo>>(
                            Option<Todo>.None))))
        select response;
}