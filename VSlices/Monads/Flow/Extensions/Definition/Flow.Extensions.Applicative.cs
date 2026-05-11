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
    public sealed partial class Flow<C, R, A>
    {
        public Flow<C, R, O> Action<O>(K<Flow<C, R>, O> mo) =>
            Flow<C, R>.Action(this, mo);
        
        public Flow<C, R, Seq<A>> Replicate(int count) =>
            Flow<C, R>.Replicate(count, this);
    }

    public partial class Flow<C, R>
    {
        public static Flow<C, R, A> BackAction<A, O>(K<Flow<C, R>, A> ma, K<Flow<C, R>, O> mo) =>
            Action(mo, ma);

        public static Flow<C, R, Seq<A>> Replicate<A>(int count, K<Flow<C, R>, A> ma) =>
            +Applicative.replicate(count, ma);

        public static Flow<C, R, Option<O>> When<O>(
            K<Flow<C, R>, bool> ma,
            K<Flow<C, R>, O> Then) =>
            ma.Bind(b => b ? Then.Bind(Some) : None<O>());

        public static Flow<C, R, Unit> When(
            K<Flow<C, R>, bool> m,
            K<Flow<C, R>, Unit> Then) =>
            When<Unit>(m, Then).Map(_ => unit);

        public static Flow<C, R, Option<O>> When<O>(
            K<Flow<C, R>, bool> ma,
            K<IO, O> Then) =>
            ma.Bind(b => b ? Then.As().Map(Prelude.Some) : IO.pure<Option<O>>(Option.None));

        public static Flow<C, R, Unit> When(
            K<Flow<C, R>, bool> m,
            K<IO, Unit> Then) =>
            When<Unit>(m, Then).Map(_ => unit);

        public static Flow<C, R, Option<O>> When<O>(
            K<Flow<C, R>, bool> ma,
            K<Eff, O> Then) =>
            ma.Bind(b => b ? Then.As().Map(Prelude.Some) : Eff.Success<Option<O>>(Option.None));

        public static Flow<C, R, Unit> When(
            K<Flow<C, R>, bool> m,
            K<Eff, Unit> Then) =>
            When<Unit>(m, Then).Map(_ => unit);

        public static Flow<C, R, Option<O>> When<O>(
            K<Flow<C, R>, bool> ma,
            K<Eff<C>, O> Then) =>
            ma.Bind(b => b ? Then.As().Map(Prelude.Some) : Eff.Success<Option<O>>(Option.None));

        public static Flow<C, R, Unit> When(
            K<Flow<C, R>, bool> m,
            K<Eff<C>, Unit> Then) =>
            When<Unit>(m, Then).Map(_ => unit);

        public static Flow<C, R, Option<O>> When<O>(
            K<Flow<C, R>, bool> ma,
            K<Fin, O> Then) =>
            ma.Bind(b => b ? Then.As().Map(Prelude.Some) : Eff.Success<C, Option<O>>(Option.None));

        public static Flow<C, R, Unit> When(
            K<Flow<C, R>, bool> m,
            K<Fin, Unit> Then) =>
            When<Unit>(m, Then).Map(_ => unit);

        public static Flow<C, R, Option<O>> When<O>(
            K<Flow<C, R>, bool> ma,
            K<FinT<IO>, O> Then) =>
            ma.Bind(b => b ? Then.As().Map(Prelude.Some) : FinT.Succ<IO, Option<O>>(Option.None));

        public static Flow<C, R, Unit> When(
            K<Flow<C, R>, bool> m,
            K<FinT<IO>, Unit> Then) =>
            When<Unit>(m, Then).Map(_ => unit);

        public static Flow<C, R, Option<O>> When<O>(
            K<Flow<C, R>, bool> ma,
            K<FinT<Eff>, O> Then) =>
            ma.Bind(b => b ? Then.As().Map(Prelude.Some) : FinT.Succ<Eff, Option<O>>(Option.None));

        public static Flow<C, R, Unit> When(
            K<Flow<C, R>, bool> m,
            K<FinT<Eff>, Unit> Then) =>
            When<Unit>(m, Then).Map(_ => unit);

        public static Flow<C, R, Option<O>> When<O>(
            K<Flow<C, R>, bool> ma,
            K<FinT<Eff<C>>, O> Then) =>
            ma.Bind(b => b ? Then.As().Map(Prelude.Some) : FinT.Succ<Eff<C>, Option<O>>(Option.None));

        public static Flow<C, R, Unit> When(
            K<Flow<C, R>, bool> m,
            K<FinT<Eff<C>>, Unit> Then) =>
            When<Unit>(m, Then).Map(_ => unit);

        public static Flow<C, R, Option<O>> Unless<O>(
            K<Flow<C, R>, bool> ma,
            K<Flow<C, R>, O> Then) =>
            ma.Bind(a => a ? None<O>() : Then.Bind(Some));

        public static Flow<C, R, Unit> Unless(
            K<Flow<C, R>, bool> ma,
            K<Flow<C, R>, Unit> Then) =>
            Unless<Unit>(ma, Then).Map(_ => unit);
    }
}