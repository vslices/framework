// Resharper disable CheckNamespace


namespace LanguageExt
{
    public static partial class EffModuleExtensions
    {
        extension<RT, A, B>(Eff<RT>)
        {
            public static Eff<RT, B> Bind(
                K<Eff<RT>, A> ma,
                Func<A, Fin<B>> fb) =>
                ma.As().Bind<B>(a => fb(a).ToEff());

            public static Eff<RT, B> Bind(
                K<Eff<RT>, A> ma,
                Func<A, K<Fin, B>> fb) =>
                Eff<RT>.Bind(ma, a => +fb(a));
        }
    }

    public static partial class EffFluentAPISyntax
    {
        extension<RT, A>(K<Eff<RT>, A> ma)
        {
            public Eff<RT, B> Bind<B>(Func<A, Fin<B>> fb) =>
                Eff<RT>.Bind(ma, fb);
            public Eff<RT, B> Bind<B>(Func<A, K<Fin, B>> fb) =>
                Eff<RT>.Bind(ma, fb);
        }
    }

    public static partial class EffLinqSyntax
    {
        extension<RT, A>(K<Eff<RT>, A> ma)
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
                Func<A, Fin<B>> fb,
                Func<A, B, C> fc) =>
                Eff<RT>.Bind(ma, a => fb(a).Map(b => fc(a, b)));

            /// <summary>
            ///
            /// </summary>
            /// <typeparam name="A"></typeparam>
            /// <typeparam name="B"></typeparam>
            /// <param name="fb"></param>
            /// <param name="fc"></param>
            /// <returns></returns>
            public Eff<RT, C> SelectMany<B, C>(
                Func<A, K<Fin, B>> fb,
                Func<A, B, C> fc) =>
                ma.Bind(a => fb(a).Map(b => fc(a, b)));
        }

        extension<RT, A>(K<Eff<RT>, A> ma)
        {
            public Eff<RT, C> SelectMany<RQ, B, C>(
                Func<A, Fin<B>> fb,
                Func<A, B, C> fc) =>
                ma.Bind(a => fb(a).Map(b => fc(a, b)));

            public Eff<RT, C> SelectMany<RQ, B, C>(
                Func<A, K<Fin, B>> fb,
                Func<A, B, C> fc) =>
                ma.Bind(a => fb(a).Map(b => fc(a, b)));
        }
    }

    public static partial class EffOperatorSyntax
    {
        extension<RT, A, B>(K<Eff<RT>, A>)
        {

            /// <summary>
            /// 
            /// </summary>
            /// <param name="ma"></param>
            /// <param name="f"></param>
            /// <returns></returns>
            public static Eff<RT, B> operator >>(
                K<Eff<RT>, A> ma,
                Func<A, K<Fin, B>> f) =>
                ma.Bind(f);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="ma"></param>
            /// <param name="f"></param>
            /// <returns></returns>
            public static Eff<RT, B> operator >>(
                K<Eff<RT>, A> ma,
                Func<A, Fin<B>> f) =>
                ma.Bind(f);
        }
    }
}

