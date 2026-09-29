// Resharper disable CheckNamespace
using VSlices.Monads;

namespace VSlices.Monads
{
    public partial class Flow<ALG, RQ>
    {
        public static Flow<ALG, RQ, B> Bind<A, B>(
            K<Flow<ALG, RQ>, A> ma,
            Func<A, IO<B>> fb) =>
            ma.Bind(a => new Flow<ALG, RQ, B>(fb(a)));
        
        public static Flow<ALG, RQ, B> Bind<A, B>(
            K<Flow<ALG, RQ>, A> ma,
            Func<A, K<IO, B>> fb) =>
            Bind(ma, a => +fb(a));
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
            public Flow<ALG, RQ, B> Bind<B>(Func<A, K<IO, B>> fb) =>
                Flow<ALG, RQ>.Bind(ma, fb);

            /// <summary>
            ///
            /// </summary>
            /// <typeparam name="B"></typeparam>
            /// <param name="fb"></param>
            /// <returns></returns>
            public Flow<ALG, RQ, B> Bind<B>(Func<A, IO<B>> fb) =>
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
                Func<A, IO<B>> bind,
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
                Func<A, K<IO, B>> bind,
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
                Func<A, K<IO, B>> f) =>
                ma.Bind(f);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="ma"></param>
            /// <param name="f"></param>
            /// <returns></returns>
            public static Flow<ALG, RQ, B> operator >>(
                K<Flow<ALG, RQ>, A> ma,
                Func<A, IO<B>> f) =>
                ma.Bind(f);
        }
    }
}
