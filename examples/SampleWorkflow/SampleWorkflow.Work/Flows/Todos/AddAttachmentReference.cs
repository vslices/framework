using LanguageExt;
using LanguageExt.Common;
using LanguageExt.Traits;
using SampleWorkflow.Spaces;
using VSlices.Space;
using VSlices.Work;
using SampleWorkflow.Work.Algebras;
using static LanguageExt.Prelude;

namespace SampleWorkflow.Work;

/// <summary>
/// Associates an already-existing external resource with a Todo.
/// This WorkFlow does not know which service owns the referenced resource.
/// </summary>
public sealed class AddAttachmentReference :
    Feature<
        AddAttachmentReference,
        TodoAlgebra,
        AddAttachmentReference.Request,
        AddAttachmentReference.Response>
{
    public sealed record Request(
        TodoId Id,
        ResourceReference Resource);

    public sealed record Response(
        Either<Error, Option<Todo>> Todo);
    
    public static Free<TodoAlgebra, Response> Get(Request request) =>
        from current in PointReader.read<TodoAlgebra, Todo, TodoId>(request.Id)
        from response in current.Match(
            Some: todo =>
            {
                if (todo.Attachments.Contains(request.Resource))
                {
                    return Free.pure<TodoAlgebra, Response>(
                        new Response(
                            Either.Right<Error, Option<Todo>>(Some(todo))));
                }

                return todo.Update(state => state with
                    {
                        Attachments =
                            [.. state.Attachments, request.Resource]
                    })
                    .Match(
                        Succ: updated =>
                            from _ in PointWriter.write<TodoAlgebra, Todo>(updated)
                            select new Response(
                                Either.Right<Error, Option<Todo>>(
                                    Some(updated))),
                        Fail: error =>
                            Free.pure<TodoAlgebra, Response>(
                                new Response(
                                    Either.Left<Error, Option<Todo>>(
                                        error))));
            },
            None: static () =>
                Free.pure<TodoAlgebra, Response>(
                    new Response(
                        Either.Right<Error, Option<Todo>>(
                            Option<Todo>.None))))
        select response;
}
