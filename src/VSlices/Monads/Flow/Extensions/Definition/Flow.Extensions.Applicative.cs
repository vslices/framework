// Resharper disable CheckNamespace
using VSlices.Monads;

namespace VSlices
{
    public static partial class FlowExtensions
    {
        extension<C, R, A>(K<Flow<C, R>, A> ma)
        {
            public Flow<C, R, B> Action<B>(K<Flow<C, R>, B> mb) =>
                ma.As().Action(mb);

            public Flow<C, R, Seq<A>> Replicate(int count) =>
                ma.As().Replicate(count);
        }
        
        extension<C, R, I, O>(K<Flow<C, R>, Func<I, O>> mf)
        {
            public Flow<C, R, O> Apply(K<Flow<C, R>, I> ma) =>
                Flow<C, R>.Apply(mf, ma);
        }

        extension<C, R, I, O>(K<Flow<C, R>, Func<I, K<Flow<C, R>, O>>> mf)
        {
            public Flow<C, R, O> ApplyM(K<Flow<C, R>, I> mo) =>
                Flow<C, R>.Apply(mf, mo).Flatten();
        }

        extension<C, R, I, O>(K<Flow<C, R>, Func<I, Flow<C, R, O>>> mf)
        {
            public Flow<C, R, O> ApplyM(K<Flow<C, R>, I> mo) =>
                Flow<C, R>.Apply(mf, mo).Flatten();
        }
    }
}

namespace VSlices.Monads
{
    public sealed partial class Flow<RT, REQ, RES>
    {
        public Flow<RT, REQ, O> Action<O>(K<Flow<RT, REQ>, O> mo) =>
            Flow<RT, REQ>.Action(this, mo);
        
        public Flow<RT, REQ, Seq<RES>> Replicate(int count) =>
            Flow<RT, REQ>.Replicate(count, this);
    }

    public partial class Flow<RT, REQ>
    {
        public static Flow<RT, REQ, A> BackAction<A, O>(K<Flow<RT, REQ>, A> ma, K<Flow<RT, REQ>, O> mo) =>
            Action(mo, ma);

        public static Flow<RT, REQ, Seq<A>> Replicate<A>(int count, K<Flow<RT, REQ>, A> ma) =>
            +Applicative.replicate(count, ma);

        public static Flow<RT, REQ, Option<O>> When<O>(
            K<Flow<RT, REQ>, bool> ma,
            K<Flow<RT, REQ>, O> Then) =>
            ma.Bind(b => b ? Then.Bind(Some) : None<O>());

        public static Flow<RT, REQ, Unit> When(
            K<Flow<RT, REQ>, bool> m,
            K<Flow<RT, REQ>, Unit> Then) =>
            When<Unit>(m, Then).Map(_ => unit);

        public static Flow<RT, REQ, Option<O>> When<O>(
            K<Flow<RT, REQ>, bool> ma,
            K<IO, O> Then) =>
            ma.Bind(b => b ? Then.As().Map(Prelude.Some) : IO.pure<Option<O>>(Option.None));

        public static Flow<RT, REQ, Unit> When(
            K<Flow<RT, REQ>, bool> m,
            K<IO, Unit> Then) =>
            When<Unit>(m, Then).Map(_ => unit);

        public static Flow<RT, REQ, Option<O>> When<O>(
            K<Flow<RT, REQ>, bool> ma,
            K<Eff, O> Then) =>
            ma.Bind(b => b ? Then.As().Map(Prelude.Some) : Eff.Success<Option<O>>(Option.None));

        public static Flow<RT, REQ, Unit> When(
            K<Flow<RT, REQ>, bool> m,
            K<Eff, Unit> Then) =>
            When<Unit>(m, Then).Map(_ => unit);

        public static Flow<RT, REQ, Option<O>> When<O>(
            K<Flow<RT, REQ>, bool> ma,
            K<Eff<RT>, O> Then) =>
            ma.Bind(b => b ? Then.As().Map(Prelude.Some) : Eff.Success<Option<O>>(Option.None));

        public static Flow<RT, REQ, Unit> When(
            K<Flow<RT, REQ>, bool> m,
            K<Eff<RT>, Unit> Then) =>
            When<Unit>(m, Then).Map(_ => unit);

        public static Flow<RT, REQ, Option<O>> When<O>(
            K<Flow<RT, REQ>, bool> ma,
            K<Fin, O> Then) =>
            ma.Bind(b => b ? Then.As().Map(Prelude.Some) : Eff.Success<RT, Option<O>>(Option.None));

        public static Flow<RT, REQ, Unit> When(
            K<Flow<RT, REQ>, bool> m,
            K<Fin, Unit> Then) =>
            When<Unit>(m, Then).Map(_ => unit);

        public static Flow<RT, REQ, Option<O>> When<O>(
            K<Flow<RT, REQ>, bool> ma,
            K<FinT<IO>, O> Then) =>
            ma.Bind(b => b ? Then.As().Map(Prelude.Some) : FinT.Succ<IO, Option<O>>(Option.None));

        public static Flow<RT, REQ, Unit> When(
            K<Flow<RT, REQ>, bool> m,
            K<FinT<IO>, Unit> Then) =>
            When<Unit>(m, Then).Map(_ => unit);

        public static Flow<RT, REQ, Option<O>> When<O>(
            K<Flow<RT, REQ>, bool> ma,
            K<FinT<Eff>, O> Then) =>
            ma.Bind(b => b ? Then.As().Map(Prelude.Some) : FinT.Succ<Eff, Option<O>>(Option.None));

        public static Flow<RT, REQ, Unit> When(
            K<Flow<RT, REQ>, bool> m,
            K<FinT<Eff>, Unit> Then) =>
            When<Unit>(m, Then).Map(_ => unit);

        public static Flow<RT, REQ, Option<O>> When<O>(
            K<Flow<RT, REQ>, bool> ma,
            K<FinT<Eff<RT>>, O> Then) =>
            ma.Bind(b => b ? Then.As().Map(Prelude.Some) : FinT.Succ<Eff<RT>, Option<O>>(Option.None));

        public static Flow<RT, REQ, Unit> When(
            K<Flow<RT, REQ>, bool> m,
            K<FinT<Eff<RT>>, Unit> Then) =>
            When<Unit>(m, Then).Map(_ => unit);

        public static Flow<RT, REQ, Option<O>> Unless<O>(
            K<Flow<RT, REQ>, bool> ma,
            K<Flow<RT, REQ>, O> Then) =>
            ma.Bind(a => a ? None<O>() : Then.Bind(Some));

        public static Flow<RT, REQ, Unit> Unless(
            K<Flow<RT, REQ>, bool> ma,
            K<Flow<RT, REQ>, Unit> Then) =>
            Unless<Unit>(ma, Then).Map(_ => unit);
    }
}