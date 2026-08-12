// Resharper disable CheckNamespace
using VSlices.Monads;

namespace VSlices
{
    /// <summary>
    ///
    /// </summary>
    public static partial class FlowExtensions
    {
        extension<C, R, A>(K<Flow<C, R>, A> ma)
        {
            /// <summary>
            ///
            /// </summary>
            /// <typeparam name="B"></typeparam>
            /// <param name="mb"></param>
            /// <returns></returns>
            public Flow<C, R, B> Action<B>(K<Flow<C, R>, B> mb) =>
                ma.As().Action(mb);

            /// <summary>
            ///
            /// </summary>
            /// <param name="count"></param>
            /// <returns></returns>
            public Flow<C, R, Seq<A>> Replicate(int count) =>
                ma.As().Replicate(count);
        }

        extension<C, R, I, O>(K<Flow<C, R>, Func<I, O>> mf)
        {
            /// <summary>
            ///
            /// </summary>
            /// <param name="ma"></param>
            /// <returns></returns>
            public Flow<C, R, O> Apply(K<Flow<C, R>, I> ma) =>
                Flow<C, R>.Apply(mf, ma);
        }

        extension<C, R, I, O>(K<Flow<C, R>, Func<I, K<Flow<C, R>, O>>> mf)
        {
            /// <summary>
            ///
            /// </summary>
            /// <param name="mo"></param>
            /// <returns></returns>
            public Flow<C, R, O> ApplyM(K<Flow<C, R>, I> mo) =>
                Flow<C, R>.Apply(mf, mo).Flatten();
        }

        extension<C, R, I, O>(K<Flow<C, R>, Func<I, Flow<C, R, O>>> mf)
        {
            /// <summary>
            ///
            /// </summary>
            /// <param name="mo"></param>
            /// <returns></returns>
            public Flow<C, R, O> ApplyM(K<Flow<C, R>, I> mo) =>
                Flow<C, R>.Apply(mf, mo).Flatten();
        }
    }
}

namespace VSlices.Monads
{
    public sealed partial class Flow<RT, REQ, RES>
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="O"></typeparam>
        /// <param name="mo"></param>
        /// <returns></returns>
        public Flow<RT, REQ, O> Action<O>(K<Flow<RT, REQ>, O> mo) =>
            Flow<RT, REQ>.Action(this, mo);

        /// <summary>
        ///
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public Flow<RT, REQ, Seq<RES>> Replicate(int count) =>
            Flow<RT, REQ>.Replicate(count, this);
    }

    public partial class Flow<RT, REQ>
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="A"></typeparam>
        /// <typeparam name="O"></typeparam>
        /// <param name="ma"></param>
        /// <param name="mo"></param>
        /// <returns></returns>
        public static Flow<RT, REQ, A> BackAction<A, O>(K<Flow<RT, REQ>, A> ma, K<Flow<RT, REQ>, O> mo) =>
            Action(mo, ma);

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="A"></typeparam>
        /// <param name="count"></param>
        /// <param name="ma"></param>
        /// <returns></returns>
        public static Flow<RT, REQ, Seq<A>> Replicate<A>(int count, K<Flow<RT, REQ>, A> ma) =>
            +Applicative.replicate(count, ma);

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="O"></typeparam>
        /// <param name="ma"></param>
        /// <param name="Then"></param>
        /// <returns></returns>
        public static Flow<RT, REQ, Option<O>> When<O>(
            K<Flow<RT, REQ>, bool> ma,
            K<Flow<RT, REQ>, O> Then) =>
            ma.Bind(b => b ? Then.Bind(Some) : None<O>());

        /// <summary>
        ///
        /// </summary>
        /// <param name="m"></param>
        /// <param name="Then"></param>
        /// <returns></returns>
        public static Flow<RT, REQ, Unit> When(
            K<Flow<RT, REQ>, bool> m,
            K<Flow<RT, REQ>, Unit> Then) =>
            When<Unit>(m, Then).Map(_ => unit);

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="O"></typeparam>
        /// <param name="ma"></param>
        /// <param name="Then"></param>
        /// <returns></returns>
        public static Flow<RT, REQ, Option<O>> When<O>(
            K<Flow<RT, REQ>, bool> ma,
            K<IO, O> Then) =>
            ma.Bind(b => b ? Then.As().Map(Prelude.Some) : IO.pure<Option<O>>(Option.None));

        /// <summary>
        ///
        /// </summary>
        /// <param name="m"></param>
        /// <param name="Then"></param>
        /// <returns></returns>
        public static Flow<RT, REQ, Unit> When(
            K<Flow<RT, REQ>, bool> m,
            K<IO, Unit> Then) =>
            When<Unit>(m, Then).Map(_ => unit);

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="O"></typeparam>
        /// <param name="ma"></param>
        /// <param name="Then"></param>
        /// <returns></returns>
        public static Flow<RT, REQ, Option<O>> When<O>(
            K<Flow<RT, REQ>, bool> ma,
            K<Eff, O> Then) =>
            ma.Bind(b => b ? Then.As().Map(Prelude.Some) : Eff.Success<Option<O>>(Option.None));

        /// <summary>
        ///
        /// </summary>
        /// <param name="m"></param>
        /// <param name="Then"></param>
        /// <returns></returns>
        public static Flow<RT, REQ, Unit> When(
            K<Flow<RT, REQ>, bool> m,
            K<Eff, Unit> Then) =>
            When<Unit>(m, Then).Map(_ => unit);

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="O"></typeparam>
        /// <param name="ma"></param>
        /// <param name="Then"></param>
        /// <returns></returns>
        public static Flow<RT, REQ, Option<O>> When<O>(
            K<Flow<RT, REQ>, bool> ma,
            K<Eff<RT>, O> Then) =>
            ma.Bind(b => b ? Then.As().Map(Prelude.Some) : Eff.Success<Option<O>>(Option.None));

        /// <summary>
        ///
        /// </summary>
        /// <param name="m"></param>
        /// <param name="Then"></param>
        /// <returns></returns>
        public static Flow<RT, REQ, Unit> When(
            K<Flow<RT, REQ>, bool> m,
            K<Eff<RT>, Unit> Then) =>
            When<Unit>(m, Then).Map(_ => unit);

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="O"></typeparam>
        /// <param name="ma"></param>
        /// <param name="Then"></param>
        /// <returns></returns>
        public static Flow<RT, REQ, Option<O>> When<O>(
            K<Flow<RT, REQ>, bool> ma,
            K<Fin, O> Then) =>
            ma.Bind(b => b ? Then.As().Map(Prelude.Some) : Eff.Success<RT, Option<O>>(Option.None));

        /// <summary>
        ///
        /// </summary>
        /// <param name="m"></param>
        /// <param name="Then"></param>
        /// <returns></returns>
        public static Flow<RT, REQ, Unit> When(
            K<Flow<RT, REQ>, bool> m,
            K<Fin, Unit> Then) =>
            When<Unit>(m, Then).Map(_ => unit);

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="O"></typeparam>
        /// <param name="ma"></param>
        /// <param name="Then"></param>
        /// <returns></returns>
        public static Flow<RT, REQ, Option<O>> When<O>(
            K<Flow<RT, REQ>, bool> ma,
            K<FinT<IO>, O> Then) =>
            ma.Bind(b => b ? Then.As().Map(Prelude.Some) : FinT.Succ<IO, Option<O>>(Option.None));

        /// <summary>
        ///
        /// </summary>
        /// <param name="m"></param>
        /// <param name="Then"></param>
        /// <returns></returns>
        public static Flow<RT, REQ, Unit> When(
            K<Flow<RT, REQ>, bool> m,
            K<FinT<IO>, Unit> Then) =>
            When<Unit>(m, Then).Map(_ => unit);

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="O"></typeparam>
        /// <param name="ma"></param>
        /// <param name="Then"></param>
        /// <returns></returns>
        public static Flow<RT, REQ, Option<O>> When<O>(
            K<Flow<RT, REQ>, bool> ma,
            K<FinT<Eff>, O> Then) =>
            ma.Bind(b => b ? Then.As().Map(Prelude.Some) : FinT.Succ<Eff, Option<O>>(Option.None));

        /// <summary>
        ///
        /// </summary>
        /// <param name="m"></param>
        /// <param name="Then"></param>
        /// <returns></returns>
        public static Flow<RT, REQ, Unit> When(
            K<Flow<RT, REQ>, bool> m,
            K<FinT<Eff>, Unit> Then) =>
            When<Unit>(m, Then).Map(_ => unit);

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="O"></typeparam>
        /// <param name="ma"></param>
        /// <param name="Then"></param>
        /// <returns></returns>
        public static Flow<RT, REQ, Option<O>> When<O>(
            K<Flow<RT, REQ>, bool> ma,
            K<FinT<Eff<RT>>, O> Then) =>
            ma.Bind(b => b ? Then.As().Map(Prelude.Some) : FinT.Succ<Eff<RT>, Option<O>>(Option.None));

        /// <summary>
        ///
        /// </summary>
        /// <param name="m"></param>
        /// <param name="Then"></param>
        /// <returns></returns>
        public static Flow<RT, REQ, Unit> When(
            K<Flow<RT, REQ>, bool> m,
            K<FinT<Eff<RT>>, Unit> Then) =>
            When<Unit>(m, Then).Map(_ => unit);

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="O"></typeparam>
        /// <param name="ma"></param>
        /// <param name="Then"></param>
        /// <returns></returns>
        public static Flow<RT, REQ, Option<O>> Unless<O>(
            K<Flow<RT, REQ>, bool> ma,
            K<Flow<RT, REQ>, O> Then) =>
            ma.Bind(a => a ? None<O>() : Then.Bind(Some));

        /// <summary>
        ///
        /// </summary>
        /// <param name="ma"></param>
        /// <param name="Then"></param>
        /// <returns></returns>
        public static Flow<RT, REQ, Unit> Unless(
            K<Flow<RT, REQ>, bool> ma,
            K<Flow<RT, REQ>, Unit> Then) =>
            Unless<Unit>(ma, Then).Map(_ => unit);
    }
}
