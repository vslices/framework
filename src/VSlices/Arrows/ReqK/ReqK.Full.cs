namespace VSlices.Arrows;

public partial class ReqK<M, IN>
    where M : Monad<M>
{
    /// <summary>
    /// Represents a full endomorphic effectful requirement pipeline.
    /// </summary>
    public readonly record struct Full(
        ReqK<M, IN, IN, IN, IN> Value)
    {
        public FinT<M, IN> RunFinT(IN input) =>
            Value.RunFinT(input);

        public EitherT<Error, M, ReqState<IN>> RawRun(IN input) =>
            Value.RawRun(input);

        public ReqK<M, IN>.Full Apply<I2, O2>(
            ReqK<M, IN, IN, I2, O2> rules,
            Func<IN, I2> To) =>
            Value.Apply(rules, To);

        public static implicit operator Full(
            ReqK<M, IN, IN, IN, IN> value) =>
            new(value);

        public static implicit operator ReqK<M, IN, IN, IN, IN>(
            Full value) =>
            value.Value;
    }
}

public partial class ReqK<M, IN, OUT>
    where M : Monad<M>
{
    /// <summary>
    /// Represents a full effectful requirement pipeline whose local
    /// input/output coincide with the pipeline input/output.
    /// </summary>
    public readonly record struct Full(
        ReqK<M, IN, OUT, IN, OUT> Value)
    {
        public FinT<M, OUT> RunFinT(IN input) =>
            Value.RunFinT(input);

        public EitherT<Error, M, ReqState<OUT>> RawRun(IN input) =>
            Value.RawRun(input);

        public ReqK<M, IN, OUT>.Full Apply<I2, O2>(
            ReqK<M, IN, OUT, I2, O2> rules,
            Func<OUT, I2> To) =>
            Value.Apply(rules, To);

        public static implicit operator Full(
            ReqK<M, IN, OUT, IN, OUT> value) =>
            new(value);

        public static implicit operator ReqK<M, IN, OUT, IN, OUT>(
            Full value) =>
            value.Value;
    }
}
