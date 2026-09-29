using VSlices.Monads;

namespace VSlices;

public static partial class FlowFluentAPISyntax
{
    extension<ALG, RQ, A>(K<Flow<ALG, RQ>, A> ma)
    {
        public Flow<ALG, RQ, B> Bind<B>(
            Func<A, Derive<RQ, B>> bind) =>
            new((algebra, request) =>
                ma.RunFlow(algebra, request)
                    .Bind(a => bind(a).Run(request)));

        public Flow<ALG, RQ, B> Bind<B>(
            Func<A, Compute<ALG, B>> bind) =>
            new((algebra, request) =>
                ma.RunFlow(algebra, request)
                    .Bind(a => bind(a).Run(algebra)));
    }
}

public static partial class FlowLinqSyntax
{
    extension<ALG, RQ, A>(K<Flow<ALG, RQ>, A> ma)
    {
        public Flow<ALG, RQ, C> SelectMany<B, C>(
            Func<A, Derive<RQ, B>> bind,
            Func<A, B, C> project) =>
            new((algebra, request) =>
                ma.RunFlow(algebra, request)
                    .Bind(a =>
                        bind(a)
                            .Run(request)
                            .Map(b => project(a, b))));

        public Flow<ALG, RQ, C> SelectMany<B, C>(
            Func<A, Compute<ALG, B>> bind,
            Func<A, B, C> project) =>
            new((algebra, request) =>
                ma.RunFlow(algebra, request)
                    .Bind(a =>
                        bind(a)
                            .Run(algebra)
                            .Map(b => project(a, b))));
    }
}
