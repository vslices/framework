// Resharper disable CheckNamespace
using VSlices.Monads;

namespace VSlices;

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
        public Flow<ALG, RQ, B> Bind<B>(Func<A, K<Flow<ALG, RQ>, B>> fb) =>
            Flow<ALG, RQ>.Bind(ma, fb);

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="B"></typeparam>
        /// <param name="fb"></param>
        /// <returns></returns>
        public Flow<ALG, RQ, B> Bind<B>(Func<A, Flow<ALG, RQ, B>> fb) =>
            Flow<ALG, RQ>.Bind(ma, fb);
    }

    extension<ALG, RQ, A>(K<Flow<ALG, RQ>, K<Flow<ALG, RQ>, A>> ma)
    {
        public Flow<ALG, RQ, A> Flatten<B>() =>
            Flow<ALG, RQ>.Flatten(ma);
    }

    extension<ALG, RQ, A>(K<Flow<ALG, RQ>, Flow<ALG, RQ, A>> ma)
    {
        public Flow<ALG, RQ, A> Flatten<B>() =>
            Flow<ALG, RQ>.Flatten(ma);
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
            Func<A, Flow<ALG, RQ, B>> bind,
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
            Func<A, K<Flow<ALG, RQ>, B>> bind,
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
            Func<A, K<Flow<ALG, RQ>, B>> f) =>
            ma.Bind(f);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ma"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Flow<ALG, RQ, B> operator >>(
            K<Flow<ALG, RQ>, A> ma,
            Func<A, Flow<ALG, RQ, B>> f) =>
            ma.Bind(f);
    }
}
