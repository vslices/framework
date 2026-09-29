namespace VSlices.Monads;

public sealed class Derive<RQ, A>(Func<RQ, IO<A>> run)
{
    internal IO<A> Run(RQ request) => run(request);

    public Derive<RQ, B> Map<B>(Func<A, B> map) =>
        new(request => Run(request).Map(map));

    public Derive<RQ, B> Bind<B>(Func<A, Derive<RQ, B>> bind) =>
        new(request =>
            Run(request)
                .Bind(value => bind(value).Run(request)));

    public Flow<ALG, RQ, B> Bind<ALG, B>(
        Func<A, Compute<ALG, B>> bind) =>
        new((algebra, request) =>
            Run(request)
                .Bind(value => bind(value).Run(algebra)));

    public Derive<RQ, B> Select<B>(Func<A, B> map) =>
        Map(map);

    public Derive<RQ, C> SelectMany<B, C>(
        Func<A, Derive<RQ, B>> bind,
        Func<A, B, C> project) =>
        new(request =>
            Run(request)
                .Bind(a =>
                    bind(a)
                        .Run(request)
                        .Map(b => project(a, b))));

    public Flow<ALG, RQ, C> SelectMany<ALG, B, C>(
        Func<A, Compute<ALG, B>> bind,
        Func<A, B, C> project) =>
        new((algebra, request) =>
            Run(request)
                .Bind(a =>
                    bind(a)
                        .Run(algebra)
                        .Map(b => project(a, b))));
}

public sealed class Compute<ALG, A>(Func<ALG, IO<A>> run)
{
    internal IO<A> Run(ALG algebra) => run(algebra);

    public Compute<ALG, B> Map<B>(Func<A, B> map) =>
        new(algebra => Run(algebra).Map(map));

    public Compute<ALG, B> Bind<B>(Func<A, Compute<ALG, B>> bind) =>
        new(algebra =>
            Run(algebra)
                .Bind(value => bind(value).Run(algebra)));

    public Flow<ALG, RQ, B> Bind<RQ, B>(
        Func<A, Derive<RQ, B>> bind) =>
        new((algebra, request) =>
            Run(algebra)
                .Bind(value => bind(value).Run(request)));

    public Compute<ALG, B> Select<B>(Func<A, B> map) =>
        Map(map);

    public Compute<ALG, C> SelectMany<B, C>(
        Func<A, Compute<ALG, B>> bind,
        Func<A, B, C> project) =>
        new(algebra =>
            Run(algebra)
                .Bind(a =>
                    bind(a)
                        .Run(algebra)
                        .Map(b => project(a, b))));

    public Flow<ALG, RQ, C> SelectMany<RQ, B, C>(
        Func<A, Derive<RQ, B>> bind,
        Func<A, B, C> project) =>
        new((algebra, request) =>
            Run(algebra)
                .Bind(a =>
                    bind(a)
                        .Run(request)
                        .Map(b => project(a, b))));
}

public sealed partial class Flow<ALG, RQ, A>
{
    public static implicit operator Flow<ALG, RQ, A>(
        Derive<RQ, A> derived) =>
        new((_, request) => derived.Run(request));

    public static implicit operator Flow<ALG, RQ, A>(
        Compute<ALG, A> computation) =>
        new((algebra, _) => computation.Run(algebra));
}
