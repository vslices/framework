namespace LanguageExt;

public static partial class EffFluentAPISyntax
{
    extension<RT, A>(K<Eff<RT>, A> ma)
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="B"></typeparam>
        /// <param name="fb"></param>
        /// <returns></returns>
        public Flow<RT, RQ, B> Bind<RQ, B>(Func<A, K<Flow<RT, RQ>, B>> fb) =>
            Flow<RT, RQ>.Bind(Flow<RT, RQ>.Lift(ma), fb);

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="B"></typeparam>
        /// <param name="fb"></param>
        /// <returns></returns>
        public Flow<RT, RQ, B> Bind<RQ, B>(Func<A, Flow<RT, RQ, B>> fb) =>
            Flow<RT, RQ>.Bind(Flow<RT, RQ>.Lift(ma), a => +fb(a));
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
        /// <param name="bind"></param>
        /// <param name="project"></param>
        /// <returns></returns>
        public Flow<RT, RQ, C> SelectMany<RQ, B, C>(
            Func<A, Flow<RT, RQ, B>> bind,
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
        public Flow<RT, RQ, C> SelectMany<RQ, B, C>(
            Func<A, K<Flow<RT, RQ>, B>> bind,
            Func<A, B, C> project) =>
            ma.Bind(x => bind(x).Map(y => project(x, y)));
    }
}

public static partial class EffOperatorSyntax
{
    extension<RT, RQ, A, B>(K<Eff<RT>, A>)
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ma"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Flow<RT, RQ, B> operator >>(
            K<Eff<RT>, A> ma,
            Func<A, K<Flow<RT, RQ>, B>> f) =>
            ma.Bind(f);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ma"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Flow<RT, RQ, B> operator >>(
            K<Eff<RT>, A> ma,
            Func<A, Flow<RT, RQ, B>> f) =>
            ma.Bind(f);
    }
}
