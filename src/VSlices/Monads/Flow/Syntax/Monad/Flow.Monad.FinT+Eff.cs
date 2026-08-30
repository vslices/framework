// Resharper disable CheckNamespace
using VSlices;
using VSlices.Monads;

namespace VSlices.Monads
{
    public partial class Flow<RT, RQ>
    {
        public static Flow<RT, RQ, A> Lift<A>(Func<RQ, FinT<Eff<RT>, A>> fa) =>
            new((rt, rq) => fa(rq).Run().Bind(ma => ma).RunIO(rt));

        public static Flow<RT, RQ, A> Lift<A>(Func<RQ, K<FinT<Eff<RT>>, A>> fa) =>
            new((rt, rq) => fa(rq).Run().Bind(ma => ma).RunIO(rt));

        public static Flow<RT, RQ, A> Lift<A>(K<FinT<Eff<RT>>, A> ma) =>
            new((rt, _) => ma.Run().Bind(ma => ma).RunIO(rt));
    }
}

namespace VSlices
{
    public static partial class VSlicesPrelude
    {
        public static Flow<RT, RQ, A> liftFlow<RT, RQ, A>(
            Func<RQ, FinT<Eff<RT>, A>> fa) =>
            Flow<RT, RQ>.Lift(fa);

        public static Flow<RT, RQ, A> liftFlow<RT, RQ, A>(
            Func<RQ, K<FinT<Eff<RT>>, A>> fa) =>
            Flow<RT, RQ>.Lift(fa);
    }
}

namespace LanguageExt
{
    public static partial class FinTEffModuleExtensions
    {
        extension<RT>(FinT<Eff<RT>>)
        {
            public static Flow<RT, RQ, B> Bind<RQ, A, B>(
                K<FinT<Eff<RT>>, A> mma,
                Func<A, Flow<RT, RQ, B>> fb) =>
                Flow<RT, RQ>.Bind(Flow<RT, RQ>.Lift(mma), fb);

            public static Flow<RT, RQ, B> Bind<RQ, A, B>(
                K<FinT<Eff<RT>>, A> ma,
                Func<A, K<Flow<RT, RQ>, B>> fb) =>
                FinT<Eff<RT>>.Bind(ma, a => +fb(a));
        }
    }

    public static partial class FinTEffFluentAPISyntax
    {
        extension<RT, A>(K<FinT<Eff<RT>>, A> ma)
        {
            public Flow<RT, RQ, B> Bind<RQ, B>(Func<A, Flow<RT, RQ, B>> fb) =>
                FinT<Eff<RT>>.Bind(ma, fb);
            
            public Flow<RT, RQ, B> Bind<RQ, B>(Func<A, K<Flow<RT, RQ>, B>> fb) =>
                FinT<Eff<RT>>.Bind(ma, fb);
        }
    }

    public static partial class FinTEffLinqSyntax
    {
        extension<RT, A>(K<FinT<Eff<RT>>, A> ma)
        {
            /// <summary>
            ///
            /// </summary>
            /// <typeparam name="A"></typeparam>
            /// <typeparam name="B"></typeparam>
            /// <param name="fb"></param>
            /// <param name="fc"></param>
            /// <returns></returns>
            public Flow<RT, RQ, C> SelectMany<RQ, B, C>(
                Func<A, Flow<RT, RQ, B>> fb,
                Func<A, B, C> fc) =>
                FinT<Eff<RT>>.Bind(ma, a => fb(a).Map(b => fc(a, b)));

            /// <summary>
            ///
            /// </summary>
            /// <typeparam name="A"></typeparam>
            /// <typeparam name="B"></typeparam>
            /// <param name="fb"></param>
            /// <param name="fc"></param>
            /// <returns></returns>
            public Flow<RT, RQ, C> SelectMany<RQ, B, C>(
                Func<A, K<Flow<RT, RQ>, B>> fb,
                Func<A, B, C> fc) =>
                ma.Bind(a => fb(a).Map(b => fc(a, b)));
        }
    }

    public static partial class FinTEffOperatorSyntax
    {
        extension<RT, RQ, A, B>(K<FinT<Eff<RT>>, A>)
        {

            /// <summary>
            /// 
            /// </summary>
            /// <param name="ma"></param>
            /// <param name="f"></param>
            /// <returns></returns>
            public static Flow<RT, RQ, B> operator >>(
                K<FinT<Eff<RT>>, A> ma,
                Func<A, K<Flow<RT, RQ>, B>> f) =>
                ma.Bind(f);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="ma"></param>
            /// <param name="f"></param>
            /// <returns></returns>
            public static Flow<RT, RQ, B> operator >>(
                K<FinT<Eff<RT>>, A> ma,
                Func<A, Flow<RT, RQ, B>> f) =>
                ma.Bind(f);
        }
    }
}

