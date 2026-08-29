// Resharper disable CheckNamespace
using VSlices.Monads;

namespace VSlices;

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
        public Flow<RT, RQ, B> Bind<B>(Func<A, K<Flow<RT, RQ>, B>> fb) =>
            Flow<RT, RQ>.Bind(ma, fb);

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="B"></typeparam>
        /// <param name="fb"></param>
        /// <returns></returns>
        public Flow<RT, RQ, B> Bind<B>(Func<A, Flow<RT, RQ, B>> fb) =>
            Flow<RT, RQ>.Bind(ma, fb);
    }

    extension<RT, RQ, A>(K<Flow<RT, RQ>, K<Flow<RT, RQ>, A>> ma)
    {
        public Flow<RT, RQ, A> Flatten<B>() =>
            Flow<RT, RQ>.Flatten(ma);
    }

    extension<RT, RQ, A>(K<Flow<RT, RQ>, Flow<RT, RQ, A>> ma)
    {
        public Flow<RT, RQ, A> Flatten<B>() =>
            Flow<RT, RQ>.Flatten(ma);
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
        /// <param name="bind"></param>
        /// <param name="project"></param>
        /// <returns></returns>
        public Flow<RT, RQ, C> SelectMany<B, C>(
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
        public Flow<RT, RQ, C> SelectMany<B, C>(
            Func<A, K<Flow<RT, RQ>, B>> bind,
            Func<A, B, C> project) =>
            ma.Bind(x => bind(x).Map(y => project(x, y)));
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
            Func<A, K<Flow<RT, RQ>, B>> f) =>
            ma.Bind(f);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ma"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Flow<RT, RQ, B> operator >>(
            K<Flow<RT, RQ>, A> ma,
            Func<A, Flow<RT, RQ, B>> f) =>
            ma.Bind(f);
    }
}
