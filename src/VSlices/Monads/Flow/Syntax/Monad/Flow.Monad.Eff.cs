// Resharper disable CheckNamespace

using System.Xml;
using VSlices.Monads;

namespace VSlices.Monads
{
    public partial class Flow<RT, RQ>
    {
        public static Flow<RT, RQ, B> Bind<A, B>(
            K<Flow<RT, RQ>, A> ma,
            Func<A, Eff<B>> fb) =>
            ma.Bind(a => new Flow<RT, RQ, B>(rt => fb(a).RunIO()));

        public static Flow<RT, RQ, B> Bind<A, B>(
            K<Flow<RT, RQ>, A> ma,
            Func<A, K<Eff, B>> fb) =>
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

        extension<RT, A>(K<Eff<RT>, A> ma)
        {
            public Flow<RT, RQ, B> Bind<RQ, B>(Func<A, Flow<RT, RQ, B>> fb) =>
                Flow<RT, RQ>.Bind(Flow<RT, RQ>.Lift(ma), fb);

            public Flow<RT, RQ, B> Bind<RQ, B>(Func<A, K<Flow<RT, RQ>, B>> fb) =>
                Flow<RT, RQ>.Bind(Flow<RT, RQ>.Lift(ma), fb);
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

        extension<RT, A>(K<Eff<RT>, A> ma)
        {
            public Flow<RT, RQ, C> SelectMany<RQ, B, C>(
                Func<A, Flow<RT, RQ, B>> fb,
                Func<A, B, C> fc) =>
                ma.Bind(a => fb(a).Map(b => fc(a, b)));

            public Flow<RT, RQ, C> SelectMany<RQ, B, C>(
                Func<A, K<Flow<RT, RQ>, B>> fb,
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

