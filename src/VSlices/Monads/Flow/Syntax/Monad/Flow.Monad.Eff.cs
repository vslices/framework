// Resharper disable CheckNamespace
using VSlices.Monads;

namespace VSlices.Monads
{
    public partial class Flow<RT, RQ>
    {
        public static Flow<RT, RQ, A> Lift<A>(Func<RQ, Eff<A>> fa) =>
            new((_, req) => fa(req).RunIO());

        public static Flow<RT, RQ, A> Lift<A>(Func<RQ, K<Eff, A>> fa) =>
            Lift(a => +fa(a));

        public static Flow<RT, RQ, A> Lift<A>(K<Eff, A> ma) =>
            Lift(_ => ma);

        public static Flow<RT, RQ, B> Bind<A, B>(
            K<Flow<RT, RQ>, A> ma,
            Func<A, Eff<B>> fb) =>
            Bind(ma, a => Lift(fb(a)));

        public static Flow<RT, RQ, B> Bind<A, B>(
            K<Flow<RT, RQ>, A> ma,
            Func<A, K<Eff, B>> fb) =>
            Bind(ma, a => +fb(a));
    }
}

namespace VSlices
{
    public static partial class VSlicesPrelude
    {
        public static Flow<RT, RQ, A> liftFlow<RT, RQ, A>(
            Func<RQ, Eff<A>> fa) =>
            Flow<RT, RQ>.Lift(fa);

        public static Flow<RT, RQ, A> liftFlow<RT, RQ, A>(
            Func<RQ, K<Eff, A>> fa) =>
            Flow<RT, RQ>.Lift(fa);
    }
    
    public static partial class FlowFluentAPISyntax
    {
        extension<RT, RQ, A>(K<Flow<RT, RQ>, A> ma)
        {
            /// <summary>
            ///
            /// </summary>
            /// <typeparam name="B"></typeparam>
            /// <param name="fb"></param>
            /// <returns></returns>
            public Flow<RT, RQ, B> Bind<B>(Func<A, K<Eff, B>> fb) =>
                Flow<RT, RQ>.Bind(ma, fb);

            /// <summary>
            ///
            /// </summary>
            /// <typeparam name="B"></typeparam>
            /// <param name="fb"></param>
            /// <returns></returns>
            public Flow<RT, RQ, B> Bind<B>(Func<A, Eff<B>> fb) =>
                Flow<RT, RQ>.Bind(ma, fb);
        }
    }

    public static partial class FlowLinqSyntax
    {
        extension<RT, RQ, A>(K<Flow<RT, RQ>, A> ma)
        {
            /// <summary>
            ///
            /// </summary>
            /// <typeparam name="A"></typeparam>
            /// <typeparam name="B"></typeparam>
            /// <param name="fb"></param>
            /// <param name="fc"></param>
            /// <returns></returns>
            public Flow<RT, RQ, C> SelectMany<B, C>(
                Func<A, Eff<B>> fb,
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
            public Flow<RT, RQ, C> SelectMany<B, C>(
                Func<A, K<Eff, B>> fb,
                Func<A, B, C> fc) =>
                ma.Bind(a => fb(a).Map(b => fc(a, b)));
        }
    }

    public static partial class FlowOperatorSyntax
    {
        extension<RT, RQ, A, B>(K<Flow<RT, RQ>, A>)
        {

            /// <summary>
            /// 
            /// </summary>
            /// <param name="ma"></param>
            /// <param name="f"></param>
            /// <returns></returns>
            public static Flow<RT, RQ, B> operator >>(
                K<Flow<RT, RQ>, A> ma,
                Func<A, K<Eff, B>> f) =>
                ma.Bind(f);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="ma"></param>
            /// <param name="f"></param>
            /// <returns></returns>
            public static Flow<RT, RQ, B> operator >>(
                K<Flow<RT, RQ>, A> ma,
                Func<A, Eff<B>> f) =>
                ma.Bind(f);
        }
    }
}

