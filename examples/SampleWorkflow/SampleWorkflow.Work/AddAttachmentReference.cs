using LanguageExt;
using LanguageExt.Common;
using LanguageExt.Traits;
using SampleWorkflow.Spaces;
using VSlices.Space;
using VSlices.Work;
using static LanguageExt.Prelude;

namespace SampleWorkflow.Work;

/// <summary>
/// Associates an already-existing external resource with a Todo.
/// This WorkFlow does not know which service owns the referenced resource.
/// </summary>
public sealed class AddAttachmentReference :
    Feature<
        AddAttachmentReference,
        AddAttachmentReference.Algebra,
        AddAttachmentReference.Request,
        AddAttachmentReference.Response>
{
    public sealed record Request(
        TodoId Id,
        ResourceReference Resource);

    public sealed record Response(
        Either<Error, Option<Todo>> Todo);

    public abstract record WorkPart<A> : K<Algebra, A>;

    public sealed record ReadPart<A>(
        TodoId Id,
        Func<Option<Todo>, A> Next) : WorkPart<A>;

    public sealed record WritePart<A>(
        Todo Point,
        Func<Unit, A> Next) : WorkPart<A>;

    public sealed class Algebra :
        Functor<Algebra>,
        PointReader<Algebra, Todo, TodoId>,
        PointWriter<Algebra, Todo>
    {
        static K<Algebra, Option<Todo>>
            PointReader<Algebra, Todo, TodoId>.Read(TodoId id) =>
            new ReadPart<Option<Todo>>(id, static point => point);

        static K<Algebra, Unit>
            PointWriter<Algebra, Todo>.Write(Todo point) =>
            new WritePart<Unit>(point, static value => value);

        static K<Algebra, B> Functor<Algebra>.Map<A, B>(
            Func<A, B> f,
            K<Algebra, A> ma) =>
            ma switch
            {
                ReadPart<A>(var id, var next) =>
                    new ReadPart<B>(id, point => f(next(point))),
                WritePart<A>(var point, var next) =>
                    new WritePart<B>(point, value => f(next(value))),
                _ => throw new NotSupportedException()
            };
    }

    public static Free<Algebra, Response> Get(Request request) =>
        from current in PointReader.read<Algebra, Todo, TodoId>(request.Id)
        from response in current.Match(
            Some: todo =>
            {
                if (todo.Attachments.Contains(request.Resource))
                {
                    return Free.pure<Algebra, Response>(
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
                            from _ in PointWriter.write<Algebra, Todo>(updated)
                            select new Response(
                                Either.Right<Error, Option<Todo>>(
                                    Some(updated))),
                        Fail: error =>
                            Free.pure<Algebra, Response>(
                                new Response(
                                    Either.Left<Error, Option<Todo>>(
                                        error))));
            },
            None: static () =>
                Free.pure<Algebra, Response>(
                    new Response(
                        Either.Right<Error, Option<Todo>>(
                            Option<Todo>.None))))
        select response;
}
