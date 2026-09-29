// Resharper disable CheckNamespace

using VSlices.Monads;

namespace VSlices.Monads
{
    public partial class Flow<ALG, RQ>
    {
        public static Flow<ALG, RQ, A> Lift<A>(Func<RQ, Fin<A>> fa) =>
            new((_, req) => fa(req).Match(Succ: IO.pure, Fail: IO.fail<A>));

        public static Flow<ALG, RQ, A> Lift<A>(Func<RQ, K<Fin, A>> fa) =>
            Lift(rq => +fa(rq));

        public static Flow<ALG, RQ, A> Lift<A>(K<Fin, A> ma) =>
            Lift(_ => ma);

        public static Flow<ALG, RQ, B> Bind<A, B>(
            K<Flow<ALG, RQ>, A> ma,
            Func<A, Fin<B>> fb) =>
            Bind(ma, a => Lift(fb(a)));

        public static Flow<ALG, RQ, B> Bind<A, B>(
            K<Flow<ALG, RQ>, A> ma,
            Func<A, K<Fin, B>> fb) =>
            Bind(ma, a => +fb(a));
    }
}

namespace VSlices
{
    public static partial class VSlicesPrelude
    {
        public static Flow<ALG, RQ, A> liftFlow<ALG, RQ, A>(
            Func<RQ, Fin<A>> fa) =>
            Flow<ALG, RQ>.Lift(fa);

        public static Flow<ALG, RQ, A> liftFlow<ALG, RQ, A>(
            Func<RQ, K<Fin, A>> fa) =>
            Flow<ALG, RQ>.Lift(fa);
    }
}

namespace VSlices
{
    public static partial class FlowFluentAPISyntax
    {
        extension<ALG, RQ, A>(K<Flow<ALG, RQ>, A> ma)
        {
            /// <summary>
            ///
            /// </summary>
            /// <typeparam name="B"></typeparam>
            /// <param name="fb"></param>
            /// <returns></returns>
            public Flow<ALG, RQ, B> Bind<B>(Func<A, K<Fin, B>> fb) =>
                Flow<ALG, RQ>.Bind(ma, fb);

            /// <summary>
            ///
            /// </summary>
            /// <typeparam name="B"></typeparam>
            /// <param name="fb"></param>
            /// <returns></returns>
            public Flow<ALG, RQ, B> Bind<B>(Func<A, Fin<B>> fb) =>
                Flow<ALG, RQ>.Bind(ma, fb);
        }

    }

    public static partial class FlowLinqSyntax
    {
        extension<ALG, RQ, A>(K<Flow<ALG, RQ>, A> ma)
        {
            /// <summary>
            ///
            /// </summary>
            /// <typeparam name="A"></typeparam>
            /// <typeparam name="B"></typeparam>
            /// <param name="fb"></param>
            /// <param name="fc"></param>
            /// <returns></returns>
            public Flow<ALG, RQ, C> SelectMany<B, C>(
                Func<A, Fin<B>> fb,
                Func<A, B, C> fc) =>
                ma.Bind(a => fb(a).Map(b => fc(a, b)));

            /// <summary>
            ///
            /// </summary>
            /// <typeparam name="A"></typeparam>
            /// <typeparam name="B"></typeparam>
            /// <param name="fb"></param>
            /// <param name="fc"></param>
            /// <returns></returns>
            public Flow<ALG, RQ, C> SelectMany<B, C>(
                Func<A, K<Fin, B>> fb,
                Func<A, B, C> fc) =>
                ma.Bind(a => fb(a).Map(b => fc(a, b)));
        }
    }

    public static partial class FlowOperatorSyntax
    {
        extension<ALG, RQ, A, B>(K<Flow<ALG, RQ>, A>)
        {

            /// <summary>
            /// 
            /// </summary>
            /// <param name="ma"></param>
            /// <param name="f"></param>
            /// <returns></returns>
            public static Flow<ALG, RQ, B> operator >>(
                K<Flow<ALG, RQ>, A> ma,
                Func<A, K<Fin, B>> f) =>
                ma.Bind(f);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="ma"></param>
            /// <param name="f"></param>
            /// <returns></returns>
            public static Flow<ALG, RQ, B> operator >>(
                K<Flow<ALG, RQ>, A> ma,
                Func<A, Fin<B>> f) =>
                ma.Bind(f);
        }
    }
}

