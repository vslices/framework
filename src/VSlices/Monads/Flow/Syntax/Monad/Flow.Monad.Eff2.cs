// Resharper disable CheckNamespace
using VSlices.Monads;

namespace VSlices.Monads
{
    public partial class Flow<RT, RQ>
    {
        public static Flow<RT, RQ, B> Bind<A, B>(
            K<Flow<RT, RQ>, A> ma,
            Func<A, Eff<RT, B>> fb) =>
            ma.Bind(a => new Flow<RT, RQ, B>(rt => fb(a).RunIO(rt)));
        
        public static Flow<RT, RQ, B> Bind<A, B>(
            K<Flow<RT, RQ>, A> ma,
            Func<A, K<Eff<RT>, B>> fb) =>
            Bind(ma, a => +fb(a));
    }
}

namespace VSlices
{
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
            public Flow<RT, RQ, B> Bind<B>(Func<A, K<Eff<RT>, B>> fb) =>
                Flow<RT, RQ>.Bind(ma, fb);

            /// <summary>
            ///
            /// </summary>
            /// <typeparam name="B"></typeparam>
            /// <param name="fb"></param>
            /// <returns></returns>
            public Flow<RT, RQ, B> Bind<B>(Func<A, Eff<RT, B>> fb) =>
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
            /// <param name="bind"></param>
            /// <param name="project"></param>
            /// <returns></returns>
            public Flow<RT, RQ, C> SelectMany<B, C>(
                Func<A, Eff<RT, B>> bind,
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
                Func<A, K<Eff<RT>, B>> bind,
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
                Func<A, K<Eff<RT>, B>> f) =>
                ma.Bind(f);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="ma"></param>
            /// <param name="f"></param>
            /// <returns></returns>
            public static Flow<RT, RQ, B> operator >>(
                K<Flow<RT, RQ>, A> ma,
                Func<A, Eff<RT, B>> f) =>
                ma.Bind(f);
        }
    }
}
