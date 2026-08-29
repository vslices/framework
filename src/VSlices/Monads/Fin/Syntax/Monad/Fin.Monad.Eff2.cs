// Resharper disable CheckNamespace


namespace LanguageExt
{
    public static partial class FinModuleExtensions
    {
        extension<RT, A, B>(Fin)
        {
            public static Eff<RT, B> Bind(
                K<Fin, A> ma,
                Func<A, Eff<RT, B>> fb) =>
                ma.As().Match(Succ: fb, Fail: Eff<RT, B>.Fail);

            public static Eff<RT, B> Bind(
                K<Fin, A> ma,
                Func<A, K<Eff<RT>, B>> fb) =>
                Fin.Bind(ma, a => +fb(a));
        }
    }

    public static partial class FinFluentAPISyntax
    {
        extension<RT, A>(K<Fin, A> ma)
        {
            /// <summary>
            ///
            /// </summary>
            /// <typeparam name="B"></typeparam>
            /// <param name="fb"></param>
            /// <returns></returns>
            public Eff<RT, B> Bind<B>(Func<A, K<Eff<RT>, B>> fb) =>
                Fin.Bind(ma, fb);

            /// <summary>
            ///
            /// </summary>
            /// <typeparam name="B"></typeparam>
            /// <param name="fb"></param>
            /// <returns></returns>
            public Eff<RT, B> Bind<B>(Func<A, Eff<RT, B>> fb) =>
                Fin.Bind(ma, fb);
        }
    }

    public static partial class FinLinqSyntax
    {
        extension<RT, A>(K<Fin, A> ma)
        {
            /// <summary>
            ///
            /// </summary>
            /// <typeparam name="A"></typeparam>
            /// <typeparam name="B"></typeparam>
            /// <param name="fb"></param>
            /// <param name="fc"></param>
            /// <returns></returns>
            public Eff<RT, C> SelectMany<B, C>(
                Func<A, Eff<RT, B>> fb,
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
            public Eff<RT, C> SelectMany<B, C>(
                Func<A, K<Eff<RT>, B>> fb,
                Func<A, B, C> fc) =>
                ma.Bind(a => fb(a).Map(b => fc(a, b)));
        }

        extension<RT, A>(K<Fin, A> ma)
        {
            public Eff<RT, C> SelectMany<RQ, B, C>(
                Func<A, Eff<RT, B>> fb,
                Func<A, B, C> fc) =>
                ma.Bind(a => fb(a).Map(b => fc(a, b)));

            public Eff<RT, C> SelectMany<RQ, B, C>(
                Func<A, K<Eff<RT>, B>> fb,
                Func<A, B, C> fc) =>
                ma.Bind(a => fb(a).Map(b => fc(a, b)));
        }
    }

    public static partial class FlowOperatorSyntax
    {
        extension<RT, A, B>(K<Fin, A>)
        {

            /// <summary>
            /// 
            /// </summary>
            /// <param name="ma"></param>
            /// <param name="f"></param>
            /// <returns></returns>
            public static Eff<RT, B> operator >>(
                K<Fin, A> ma,
                Func<A, K<Eff<RT>, B>> f) =>
                ma.Bind(f);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="ma"></param>
            /// <param name="f"></param>
            /// <returns></returns>
            public static Eff<RT, B> operator >>(
                K<Fin, A> ma,
                Func<A, Eff<RT, B>> f) =>
                ma.Bind(f);
        }
    }
}

