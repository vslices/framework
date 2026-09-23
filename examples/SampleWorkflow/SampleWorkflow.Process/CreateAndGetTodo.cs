using LanguageExt;
using LanguageExt.Common;
using SampleWorkflow.Spaces;
using SampleWorkflow.Work;
using VSlices.Work;
using Algebra = VSlices.Work.AlgebraSum<
    SampleWorkflow.Work.CreateTodo.Algebra,
    SampleWorkflow.Work.GetTodo.Algebra>;

namespace SampleWorkflow.Process;

public sealed class CreateAndGetTodo :
    Feature<
        CreateAndGetTodo,
        Algebra,
        CreateAndGetTodo.Request,
        CreateAndGetTodo.Response>
{
    public sealed record Request(TodoDetail Detail, bool Completed);
    public sealed record Response(Either<Error, Option<Todo>> Todo);

    public static Free<Algebra, Response> Get(Request request)
    {
        var create = Algebra.FromA(
            CreateTodo.Get(
                new CreateTodo.Request(
                    request.Detail,
                    request.Completed)));

        return
            from created in create
            from response in created.Todo.Match(
                Left: error =>
                    Free.pure<Algebra, Response>(
                        new Response(
                            Either.Left<Error, Option<Todo>>(error))),
                Right: maybe =>
                    maybe.Match(
                        Some: todo =>
                            Algebra.FromB(
                                GetTodo.Get(
                                    new GetTodo.Request(todo.Id)))
                                .Map(read =>
                                    new Response(
                                        Either.Right<Error, Option<Todo>>(
                                            read.Todo))),
                        None: () =>
                            Free.pure<Algebra, Response>(
                                new Response(
                                    Either.Right<Error, Option<Todo>>(
                                        Option<Todo>.None)))))
            select response;
    }
}
