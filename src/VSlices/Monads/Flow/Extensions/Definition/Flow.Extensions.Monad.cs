// Resharper disable CheckNamespace

using System.Buffers;
using System.Diagnostics;
using VSlices.Monads;

namespace VSlices.Monads
{
    public sealed partial class Flow<RT, REQ, RES>
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="B"></typeparam>
        /// <param name="fb"></param>
        /// <returns></returns>
        public Flow<RT, REQ, B> Bind<B>(Func<RES, K<Flow<RT, REQ>, B>> fb) =>
            Flow<RT, REQ>.Bind(this, fb);

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="B"></typeparam>
        /// <param name="fb"></param>
        /// <returns></returns>
        public Flow<RT, REQ, B> Bind<B>(Func<RES, Flow<RT, REQ, B>> fb) =>
            Flow<RT, REQ>.Bind(this, fb);
    }

    public partial class Flow<RT, REQ>
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="O"></typeparam>
        /// <param name="mma"></param>
        /// <returns></returns>
        public static Flow<RT, REQ, O> Flatten<O>(K<Flow<RT, REQ>, K<Flow<RT, REQ>, O>> mma) =>
            +Monad.flatten(mma);

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="A"></typeparam>
        /// <typeparam name="B"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Flow<RT, REQ, Next<A, B>> Done<A, B>(B value) =>
            Pure(Next.Done<A, B>(value));

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="A"></typeparam>
        /// <typeparam name="B"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Flow<RT, REQ, Next<A, B>> Loop<A, B>(A value) =>
            Pure(Next.Loop<A, B>(value));
    }
}

namespace VSlices
{
    public static partial class FlowExtensions
    {
        extension<C, R, I>(K<Flow<C, R>, I> ma)
        {
            /// <summary>
            ///
            /// </summary>
            /// <typeparam name="O"></typeparam>
            /// <param name="mf"></param>
            /// <returns></returns>
            public Flow<C, R, O> Bind<O>(Func<I, Flow<C, R, O>> mf) =>
                ma.As().Bind(mf);

            /// <summary>
            /// 
            /// </summary>
            /// <typeparam name="O"></typeparam>
            /// <param name="mf"></param>
            /// <returns></returns>
            public Flow<C, R, O> Bind<O>(Func<I, K<Flow<C, R>, O>> mf) =>
                ma.As().Bind(mf);

            /// <summary>
            ///
            /// </summary>
            /// <typeparam name="O"></typeparam>
            /// <param name="mf"></param>
            /// <returns></returns>
            public Flow<C, R, O> Bind<O>(Func<I, Pure<O>> mf) =>
                ma.Bind(i => Flow<C, R>.Pure(mf(i)));

            /// <summary>
            ///
            /// </summary>
            /// <typeparam name="O"></typeparam>
            /// <param name="mf"></param>
            /// <returns></returns>
            public Flow<C, R, O> Bind<O>(Func<I, Error> mf) =>
                ma.Bind(i => Flow<C, R>.Fail<O>(mf(i)));

            /// <summary>
            ///
            /// </summary>
            /// <typeparam name="O"></typeparam>
            /// <param name="mf"></param>
            /// <returns></returns>
            public Flow<C, R, O> Bind<O>(Func<I, Fail<Error>> mf) =>
                ma.Bind(i => Flow<C, R>.Fail<O>(mf(i)));

            /// <summary>
            ///
            /// </summary>
            /// <typeparam name="O"></typeparam>
            /// <param name="mf"></param>
            /// <returns></returns>
            public Flow<C, R, O> Bind<O>(Func<I, Fail<string>> mf) =>
                ma.Bind(i => Flow<C, R>.Fail<O>(mf(i)));

            /// <summary>
            ///
            /// </summary>
            /// <typeparam name="O"></typeparam>
            /// <param name="mf"></param>
            /// <returns></returns>
            public Flow<C, R, O> Bind<O>(Func<I, IO<O>> mf) =>
                ma.Bind(i => Flow<C, R>.LiftIO(mf(i)));

            /// <summary>
            ///
            /// </summary>
            /// <typeparam name="O"></typeparam>
            /// <param name="mf"></param>
            /// <returns></returns>
            public Flow<C, R, O> Bind<O>(Func<I, Eff<O>> mf) =>
                ma.Bind(i => Flow<C, R>.Lift(mf(i)));

            /// <summary>
            /// 
            /// </summary>
            /// <typeparam name="O"></typeparam>
            /// <param name="mf"></param>
            /// <returns></returns>
            public Flow<C, R, O> Bind<O>(Func<I, K<Eff, O>> mf) =>
                ma.Bind(i => Flow<C, R>.Lift(mf(i)));

            /// <summary>
            ///
            /// </summary>
            /// <typeparam name="O"></typeparam>
            /// <param name="mf"></param>
            /// <returns></returns>
            public Flow<C, R, O> Bind<O>(Func<I, Eff<C, O>> mf) =>
                ma.Bind(i => Flow<C, R>.Lift(mf(i)));

            /// <summary>
            /// 
            /// </summary>
            /// <typeparam name="O"></typeparam>
            /// <param name="mf"></param>
            /// <returns></returns>
            public Flow<C, R, O> Bind<O>(Func<I, K<Eff<C>, O>> mf) =>
                ma.Bind(i => Flow<C, R>.Lift(mf(i)));

            /// <summary>
            ///
            /// </summary>
            /// <typeparam name="O"></typeparam>
            /// <param name="mf"></param>
            /// <returns></returns>
            public Flow<C, R, O> Bind<O>(Func<I, Fin<O>> mf) =>
                ma.Bind(i => Flow<C, R>.Lift(mf(i)));

            /// <summary>
            ///
            /// </summary>
            /// <typeparam name="O"></typeparam>
            /// <param name="mf"></param>
            /// <returns></returns>
            public Flow<C, R, O> Bind<O>(Func<I, FinT<IO, O>> mf) =>
                ma.Bind(i => Flow<C, R>.Lift(mf(i)));

            /// <summary>
            ///
            /// </summary>
            /// <typeparam name="O"></typeparam>
            /// <param name="mf"></param>
            /// <returns></returns>
            public Flow<C, R, O> Bind<O>(Func<I, FinT<Eff, O>> mf) =>
                ma.Bind(i => Flow<C, R>.Lift(mf(i)));

            /// <summary>
            ///
            /// </summary>
            /// <typeparam name="O"></typeparam>
            /// <param name="mf"></param>
            /// <returns></returns>
            public Flow<C, R, O> Bind<O>(Func<I, FinT<Eff<C>, O>> mf) =>
                ma.Bind(i => Flow<C, R>.Lift(mf(i)));

            /// <summary>
            ///
            /// </summary>
            /// <typeparam name="O1"></typeparam>
            /// <typeparam name="O2"></typeparam>
            /// <param name="bind"></param>
            /// <param name="project"></param>
            /// <returns></returns>
            public Flow<C, R, O2> SelectMany<O1, O2>(
                Func<I, K<Flow<C, R>, O1>> bind,
                Func<I, O1, O2> project) =>
                ma.Bind(x => bind(x).Map(y => project(x, y)));

            /// <summary>
            ///
            /// </summary>
            /// <typeparam name="O1"></typeparam>
            /// <typeparam name="O2"></typeparam>
            /// <param name="bind"></param>
            /// <param name="project"></param>
            /// <returns></returns>
            public Flow<C, R, O2> SelectMany<O1, O2>(
                Func<I, Pure<O1>> bind, Func<I, O1, O2> project) =>
                ma.Map(x => project(x, bind(x).Value));

            /// <summary>
            ///
            /// </summary>
            /// <typeparam name="O1"></typeparam>
            /// <typeparam name="O2"></typeparam>
            /// <param name="bind"></param>
            /// <param name="project"></param>
            /// <returns></returns>
            public Flow<C, R, O2> SelectMany<O1, O2>(
                Func<I, Fail<O1>> bind, Func<I, O1, O2> project) =>
                ma.Map(x => project(x, bind(x).Value));

            /// <summary>
            ///
            /// </summary>
            /// <typeparam name="O1"></typeparam>
            /// <typeparam name="O2"></typeparam>
            /// <param name="bind"></param>
            /// <param name="project"></param>
            /// <returns></returns>
            public Flow<C, R, O2> SelectMany<O1, O2>(
                Func<I, IO<O1>> bind, Func<I, O1, O2> project) =>
                ma.Bind(x => bind(x).Map(y => project(x, y)));

            /// <summary>
            ///
            /// </summary>
            /// <typeparam name="O1"></typeparam>
            /// <typeparam name="O2"></typeparam>
            /// <param name="bind"></param>
            /// <param name="project"></param>
            /// <returns></returns>
            public Flow<C, R, O2> SelectMany<O1, O2>(
                Func<I, Eff<O1>> bind, Func<I, O1, O2> project) =>
                ma.Bind(x => bind(x).Map(y => project(x, y)));

            /// <summary>
            ///
            /// </summary>
            /// <typeparam name="O1"></typeparam>
            /// <typeparam name="O2"></typeparam>
            /// <param name="bind"></param>
            /// <param name="project"></param>
            /// <returns></returns>
            public Flow<C, R, O2> SelectMany<O1, O2>(
                Func<I, Eff<C, O1>> bind, Func<I, O1, O2> project) =>
                ma.Bind(x => bind(x).Map(y => project(x, y)));

            /// <summary>
            ///
            /// </summary>
            /// <typeparam name="O1"></typeparam>
            /// <typeparam name="O2"></typeparam>
            /// <param name="bind"></param>
            /// <param name="project"></param>
            /// <returns></returns>
            public Flow<C, R, O2> SelectMany<O1, O2>(
                Func<I, Fin<O1>> bind, Func<I, O1, O2> project) =>
                ma.Bind(x => bind(x).Map(y => project(x, y)));

            /// <summary>
            ///
            /// </summary>
            /// <typeparam name="O1"></typeparam>
            /// <typeparam name="O2"></typeparam>
            /// <param name="bind"></param>
            /// <param name="project"></param>
            /// <returns></returns>
            public Flow<C, R, O2> SelectMany<O1, O2>(
                Func<I, FinT<IO, O1>> bind, Func<I, O1, O2> project) =>
                ma.Bind(x => bind(x).Map(y => project(x, y)));

            /// <summary>
            ///
            /// </summary>
            /// <typeparam name="O1"></typeparam>
            /// <typeparam name="O2"></typeparam>
            /// <param name="bind"></param>
            /// <param name="project"></param>
            /// <returns></returns>
            public Flow<C, R, O2> SelectMany<O1, O2>(
                Func<I, FinT<Eff, O1>> bind, Func<I, O1, O2> project) =>
                ma.Bind(x => bind(x).Map(y => project(x, y)));

            /// <summary>
            ///
            /// </summary>
            /// <typeparam name="O1"></typeparam>
            /// <typeparam name="O2"></typeparam>
            /// <param name="bind"></param>
            /// <param name="project"></param>
            /// <returns></returns>
            public Flow<C, R, O2> SelectMany<O1, O2>(
                Func<I, FinT<Eff<C>, O1>> bind, Func<I, O1, O2> project) =>
                ma.Bind(x => bind(x).Map(y => project(x, y)));

        }

        extension<C, R, A>(K<Flow<C, R>, K<Flow<C, R>, A>> mma)
        {
            /// <summary>
            ///
            /// </summary>
            /// <returns></returns>
            public Flow<C, R, A> Flatten() =>
                Flow<C, R>.Flatten(mma);
        }

        extension<C, R, A>(K<Flow<C, R>, Flow<C, R, A>> mma)
        {
            /// <summary>
            ///
            /// </summary>
            /// <returns></returns>
            public Flow<C, R, A> Flatten() =>
                mma.Map(ma => ma.Kind()).Flatten();
        }
    }
}
