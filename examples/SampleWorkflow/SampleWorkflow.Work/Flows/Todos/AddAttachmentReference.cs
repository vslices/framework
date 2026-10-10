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
        TodoAlgebra,
        AddAttachmentReference.Request,
        AddAttachmentReference.Response>
{
    public sealed record Request(
        TodoId Id,
        ResourceReference Resource);

    public sealed record Response(
        Either<Error, Option<Todo>> Todo);
    
    public static Free<TodoAlgebra, Response> Describe(Request request) =>
        from current in TodoAlgebra.Read(request.Id)
        from response in current.Match(
            Some: todo =>
            {
                if (todo.Attachments.Contains(request.Resource))
                {
                    return TodoAlgebra.Pure(
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
                            from _ in TodoAlgebra.Write(updated)
                            select new Response(
                                Either.Right<Error, Option<Todo>>(
                                    Some(updated))),
                        Fail: error =>
                            TodoAlgebra.Pure(
                                new Response(
                                    Either.Left<Error, Option<Todo>>(
                                        error))));
            },
            None: static () =>
                TodoAlgebra.Pure(
                    new Response(
                        Either.Right<Error, Option<Todo>>(
                            Option<Todo>.None))))
        select response;
}
