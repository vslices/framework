using LanguageExt;
using LanguageExt.Common;
using LanguageExt.Traits;
using SampleWorkflow.Spaces;
using VSlices;
using VSlices.Monads;
using VSlices.Space;
using VSlices.Work;
using static LanguageExt.Prelude;

namespace SampleWorkflow.Work;

public sealed class AddAttachmentReference<RT> :
    Feature<
        AddAttachmentReference<RT>,
        RT,
        AddAttachmentReference<RT>.Request,
        AddAttachmentReference<RT>.Response>
    where RT : HasAlgebra<AddAttachmentReferenceAlgebra, RT>
{
    public sealed record Request(
        TodoId Id,
        ResourceReference Resource);

    public sealed record Response(
        Either<Error, Option<Todo>> Todo);

    public static Flow<RT, Request, Response> Get() =>
        Flow<RT, Request>.Asks(static request => request) >>
        (request => AlgebraEnv<AddAttachmentReferenceAlgebra, RT>
            .run(AddAttachmentReferenceWork.Program(request.Id, request.Resource))
            .Map(value => new Response(value)));
}

public static class AddAttachmentReferenceWork
{
    public static Free<AddAttachmentReferenceAlgebra, Either<Error, Option<Todo>>> Program(
        TodoId id,
        ResourceReference resource) =>
        from current in PointReader.read<AddAttachmentReferenceAlgebra, Todo, TodoId>(id)
        from response in current.Match(
            Some: todo =>
            {
                if (todo.Attachments.Contains(resource))
                {
                    return Free.pure<AddAttachmentReferenceAlgebra, Either<Error, Option<Todo>>>(
                        Either.Right<Error, Option<Todo>>(Some(todo)));
                }

                return todo.Update(state => state with
                    {
                        Attachments = [.. state.Attachments, resource]
                    })
                    .Match(
                        Succ: updated =>
                            from _ in PointWriter.write<AddAttachmentReferenceAlgebra, Todo>(updated)
                            select Either.Right<Error, Option<Todo>>(Some(updated)),
                        Fail: error =>
                            Free.pure<AddAttachmentReferenceAlgebra, Either<Error, Option<Todo>>>(
                                Either.Left<Error, Option<Todo>>(error)));
            },
            None: static () =>
                Free.pure<AddAttachmentReferenceAlgebra, Either<Error, Option<Todo>>>(
                    Either.Right<Error, Option<Todo>>(Option<Todo>.None)))
        select response;
}

public abstract record AddAttachmentReferenceWorkPart<A> : K<AddAttachmentReferenceAlgebra, A>;
public sealed record AddAttachmentReferenceReadPart<A>(
    TodoId Id,
    Func<Option<Todo>, A> Next) : AddAttachmentReferenceWorkPart<A>;
public sealed record AddAttachmentReferenceWritePart<A>(
    Todo Point,
    Func<Unit, A> Next) : AddAttachmentReferenceWorkPart<A>;

public sealed class AddAttachmentReferenceAlgebra :
    Functor<AddAttachmentReferenceAlgebra>,
    PointReader<AddAttachmentReferenceAlgebra, Todo, TodoId>,
    PointWriter<AddAttachmentReferenceAlgebra, Todo>
{
    static K<AddAttachmentReferenceAlgebra, Option<Todo>>
        PointReader<AddAttachmentReferenceAlgebra, Todo, TodoId>.Read(TodoId id) =>
        new AddAttachmentReferenceReadPart<Option<Todo>>(id, static point => point);

    static K<AddAttachmentReferenceAlgebra, Unit>
        PointWriter<AddAttachmentReferenceAlgebra, Todo>.Write(Todo point) =>
        new AddAttachmentReferenceWritePart<Unit>(point, static value => value);

    static K<AddAttachmentReferenceAlgebra, B> Functor<AddAttachmentReferenceAlgebra>.Map<A, B>(
        Func<A, B> f,
        K<AddAttachmentReferenceAlgebra, A> ma) =>
        ma switch
        {
            AddAttachmentReferenceReadPart<A>(var id, var next) =>
                new AddAttachmentReferenceReadPart<B>(id, point => f(next(point))),
            AddAttachmentReferenceWritePart<A>(var point, var next) =>
                new AddAttachmentReferenceWritePart<B>(point, value => f(next(value))),
            _ => throw new NotSupportedException()
        };
}
