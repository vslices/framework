// Resharper disable CheckNamespace
namespace VSlices.Monads
{
    public partial class Flow<ALG, RQ>
    {
        public static Flow<ALG, RQ, A> Lift<A>(Func<RQ, Eff<ALG, A>> fa) =>
            new((run, req) => fa(req).RunIO(run));

        public static Flow<ALG, RQ, A> Lift<A>(Func<RQ, K<Eff<ALG>, A>> fa) =>
            Lift(req => +fa(req));

        public static Flow<ALG, RQ, A> Lift<A>(K<Eff<ALG>, A> ma) =>
            Lift(_ => ma);

        public static Flow<ALG, RQ, B> Bind<A, B>(
            K<Flow<ALG, RQ>, A> ma,
            Func<A, Eff<ALG, B>> fb) =>
            Bind(ma, a => Lift(fb(a)));

        public static Flow<ALG, RQ, B> Bind<A, B>(
            K<Flow<ALG, RQ>, A> ma,
            Func<A, K<Eff<ALG>, B>> fb) =>
            Bind(ma, a => +fb(a));
    }
}

namespace VSlices
{
    public static partial class VSlicesPrelude
    {
        public static Flow<ALG, RQ, A> liftFlow<ALG, RQ, A>(
            Func<RQ, Eff<ALG, A>> fa) =>
            Flow<ALG, RQ>.Lift(fa);

        public static Flow<ALG, RQ, A> liftFlow<ALG, RQ, A>(
            Func<RQ, K<Eff<ALG>, A>> fa) =>
            Flow<ALG, RQ>.Lift(fa);
    }

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
            public Flow<ALG, RQ, B> Bind<B>(Func<A, K<Eff<ALG>, B>> fb) =>
                Flow<ALG, RQ>.Bind(ma, fb);

            /// <summary>
            ///
            /// </summary>
            /// <typeparam name="B"></typeparam>
            /// <param name="fb"></param>
            /// <returns></returns>
            public Flow<ALG, RQ, B> Bind<B>(Func<A, Eff<ALG, B>> fb) =>
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
            /// <param name="bind"></param>
            /// <param name="project"></param>
            /// <returns></returns>
            public Flow<ALG, RQ, C> SelectMany<B, C>(
                Func<A, Eff<ALG, B>> bind,
                Func<A, B, C> project) =>
                ma.Bind(x => bind(x).Map(y => project(x, y)));

            /// <summary>
            ///
            /// </summary>
            /// <typeparam name="A"></typeparam>
            /// <typeparam name="B"></typeparam>
            /// <param name="bind"></param>
            /// <param name="project"></param>
            /// <returns></returns>
            public Flow<ALG, RQ, C> SelectMany<B, C>(
                Func<A, K<Eff<ALG>, B>> bind,
                Func<A, B, C> project) =>
                ma.Bind(x => bind(x).Map(y => project(x, y)));
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
                Func<A, K<Eff<ALG>, B>> f) =>
                ma.Bind(f);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="ma"></param>
            /// <param name="f"></param>
            /// <returns></returns>
            public static Flow<ALG, RQ, B> operator >>(
                K<Flow<ALG, RQ>, A> ma,
                Func<A, Eff<ALG, B>> f) =>
                ma.Bind(f);
        }
    }
}
