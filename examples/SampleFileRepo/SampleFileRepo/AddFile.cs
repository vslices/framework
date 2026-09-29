using LanguageExt;
using LanguageExt.Traits;
using VSlices;
using VSlices.Monads;
using VSlices.Work;
using static LanguageExt.Prelude;

namespace SampleFileRepo;

public sealed class AddFile<RT> :
    Feature<AddFile<RT>, RT, AddFile<RT>.Request, AddFile<RT>.Response>
    where RT : HasAlgebra<AddFileAlgebra, RT>
{
    public sealed record Request(
        string Name,
        byte[] Content);

    public sealed record Response(SampleFile File);

    public static Flow<RT, Request, Response> Get() =>
        Flow<RT, Request>.Asks(static request => request) >>
        (request => AlgebraEnv<AddFileAlgebra, RT>
            .run(AddFileWork.Program(request.Name, request.Content))
            .Map(file => new Response(file)));
}

public static class AddFileWork
{
    private static Free<AddFileAlgebra, SampleFileId> nextId() =>
        Free.lift(AddFileAlgebra.NextId());

    public static Free<AddFileAlgebra, SampleFile> Program(
        string name,
        byte[] content) =>
        from id in nextId()
        let file = new SampleFile(id, name, [.. content])
        from _ in PointWriter.write<AddFileAlgebra, SampleFile>(file)
        select file;
}

public abstract record AddFileWorkPart<A> : K<AddFileAlgebra, A>;
public sealed record AddFileNextIdPart<A>(
    Func<SampleFileId, A> Next) : AddFileWorkPart<A>;
public sealed record AddFileWritePart<A>(
    SampleFile Point,
    Func<Unit, A> Next) : AddFileWorkPart<A>;

public sealed class AddFileAlgebra :
    Functor<AddFileAlgebra>,
    PointWriter<AddFileAlgebra, SampleFile>
{
    public static K<AddFileAlgebra, SampleFileId> NextId() =>
        new AddFileNextIdPart<SampleFileId>(static id => id);

    static K<AddFileAlgebra, Unit>
        PointWriter<AddFileAlgebra, SampleFile>.Write(SampleFile point) =>
        new AddFileWritePart<Unit>(point, static value => value);

    static K<AddFileAlgebra, B> Functor<AddFileAlgebra>.Map<A, B>(
        Func<A, B> f,
        K<AddFileAlgebra, A> ma) =>
        ma switch
        {
            AddFileNextIdPart<A>(var next) =>
                new AddFileNextIdPart<B>(id => f(next(id))),
            AddFileWritePart<A>(var point, var next) =>
                new AddFileWritePart<B>(point, value => f(next(value))),
            _ => throw new NotSupportedException()
        };
}
