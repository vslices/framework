// Resharper disable CheckNamespace
using VSlices;
using VSlices.Monads;

namespace VSlices.Monads
{
    public partial class Flow<ALG, RQ>
    {
        public static Flow<ALG, RQ, A> Lift<A>(Func<RQ, FinT<Eff<ALG>, A>> fa) =>
            new((rt, rq) => fa(rq).Run().Bind(ma => ma).RunIO(rt));

        public static Flow<ALG, RQ, A> Lift<A>(Func<RQ, K<FinT<Eff<ALG>>, A>> fa) =>
            new((rt, rq) => fa(rq).Run().Bind(ma => ma).RunIO(rt));

        public static Flow<ALG, RQ, A> Lift<A>(K<FinT<Eff<ALG>>, A> ma) =>
            new((rt, _) => ma.Run().Bind(ma => ma).RunIO(rt));
    }
}

namespace VSlices
{
    public static partial class VSlicesPrelude
    {
        public static Flow<ALG, RQ, A> liftFlow<ALG, RQ, A>(
            Func<RQ, FinT<Eff<ALG>, A>> fa) =>
            Flow<ALG, RQ>.Lift(fa);

        public static Flow<ALG, RQ, A> liftFlow<ALG, RQ, A>(
            Func<RQ, K<FinT<Eff<ALG>>, A>> fa) =>
            Flow<ALG, RQ>.Lift(fa);
    }
}

namespace LanguageExt
{
    public static partial class FinTEffModuleExtensions
    {
        extension<ALG>(FinT<Eff<ALG>>)
        {
            public static Flow<ALG, RQ, B> Bind<RQ, A, B>(
                K<FinT<Eff<ALG>>, A> mma,
                Func<A, Flow<ALG, RQ, B>> fb) =>
                Flow<ALG, RQ>.Bind(Flow<ALG, RQ>.Lift(mma), fb);

            public static Flow<ALG, RQ, B> Bind<RQ, A, B>(
                K<FinT<Eff<ALG>>, A> ma,
                Func<A, K<Flow<ALG, RQ>, B>> fb) =>
                FinT<Eff<ALG>>.Bind(ma, a => +fb(a));
        }
    }

    public static partial class FinTEffFluentAPISyntax
    {
        extension<ALG, A>(K<FinT<Eff<ALG>>, A> ma)
        {
            public Flow<ALG, RQ, B> Bind<RQ, B>(Func<A, Flow<ALG, RQ, B>> fb) =>
                FinT<Eff<ALG>>.Bind(ma, fb);
            
            public Flow<ALG, RQ, B> Bind<RQ, B>(Func<A, K<Flow<ALG, RQ>, B>> fb) =>
                FinT<Eff<ALG>>.Bind(ma, fb);
        }
    }

    public static partial class FinTEffLinqSyntax
    {
        extension<ALG, A>(K<FinT<Eff<ALG>>, A> ma)
        {
            /// <summary>
            ///
            /// </summary>
            /// <typeparam name="A"></typeparam>
            /// <typeparam name="B"></typeparam>
            /// <param name="fb"></param>
            /// <param name="fc"></param>
            /// <returns></returns>
            public Flow<ALG, RQ, C> SelectMany<RQ, B, C>(
                Func<A, Flow<ALG, RQ, B>> fb,
                Func<A, B, C> fc) =>
                FinT<Eff<ALG>>.Bind(ma, a => fb(a).Map(b => fc(a, b)));

            /// <summary>
            ///
            /// </summary>
            /// <typeparam name="A"></typeparam>
            /// <typeparam name="B"></typeparam>
            /// <param name="fb"></param>
            /// <param name="fc"></param>
            /// <returns></returns>
            public Flow<ALG, RQ, C> SelectMany<RQ, B, C>(
                Func<A, K<Flow<ALG, RQ>, B>> fb,
                Func<A, B, C> fc) =>
                ma.Bind(a => fb(a).Map(b => fc(a, b)));
        }
    }

    public static partial class FinTEffOperatorSyntax
    {
        extension<ALG, RQ, A, B>(K<FinT<Eff<ALG>>, A>)
        {

            /// <summary>
            /// 
            /// </summary>
            /// <param name="ma"></param>
            /// <param name="f"></param>
            /// <returns></returns>
            public static Flow<ALG, RQ, B> operator >>(
                K<FinT<Eff<ALG>>, A> ma,
                Func<A, K<Flow<ALG, RQ>, B>> f) =>
                ma.Bind(f);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="ma"></param>
            /// <param name="f"></param>
            /// <returns></returns>
            public static Flow<ALG, RQ, B> operator >>(
                K<FinT<Eff<ALG>>, A> ma,
                Func<A, Flow<ALG, RQ, B>> f) =>
                ma.Bind(f);
        }
    }
}

