using LanguageExt;
using LanguageExt.Common;
using SampleWorkflow.Spaces;
using SampleWorkflow.Work;
using VSlices.Work;

namespace SampleWorkflow.Process;

public sealed class CreateAndGetTodo :
    WorkProcess<
        CreateAndGetTodo,
        AlgebraSum<CreateTodo.Algebra, GetTodo.Algebra>,
        CreateAndGetTodo.Request,
        CreateAndGetTodo.Response>
{
    public sealed record Request(TodoDetail Detail, bool Completed);
    public sealed record Response(Either<Error, Option<Todo>> Todo);

    public static Free<
        AlgebraSum<CreateTodo.Algebra, GetTodo.Algebra>,
        Response> Get(Request request)
    {
        var create = FreeAlgebra.hoist<
            InjectLeft<CreateTodo.Algebra, GetTodo.Algebra>,
            CreateTodo.Algebra,
            AlgebraSum<CreateTodo.Algebra, GetTodo.Algebra>,
            CreateTodo.Response>(
                CreateTodo.Get(
                    new CreateTodo.Request(
                        request.Detail,
                        request.Completed)));

        return
            from created in create
            from response in created.Todo.Match(
                Left: error =>
                    Free.pure<
                        AlgebraSum<CreateTodo.Algebra, GetTodo.Algebra>,
                        Response>(
                        new Response(
                            Either.Left<Error, Option<Todo>>(error))),
                Right: maybe =>
                    maybe.Match(
                        Some: todo =>
                            FreeAlgebra.hoist<
                                InjectRight<CreateTodo.Algebra, GetTodo.Algebra>,
                                GetTodo.Algebra,
                                AlgebraSum<CreateTodo.Algebra, GetTodo.Algebra>,
                                GetTodo.Response>(
                                    GetTodo.Get(
                                        new GetTodo.Request(todo.Id)))
                                .Map(read =>
                                    new Response(
                                        Either.Right<Error, Option<Todo>>(
                                            read.Todo))),
                        None: () =>
                            Free.pure<
                                AlgebraSum<CreateTodo.Algebra, GetTodo.Algebra>,
                                Response>(
                                new Response(
                                    Either.Right<Error, Option<Todo>>(
                                        Option<Todo>.None)))))
            select response;
    }
}
