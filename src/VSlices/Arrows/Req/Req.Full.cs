namespace VSlices.Arrows;

public partial class Req<IN>
{
    /// <summary>
    /// Represents a full endomorphic requirement pipeline.
    /// </summary>
    public readonly record struct Full(
        Req<IN, IN, IN, IN> Value)
    {
        public Fin<IN> RunFin(IN input) =>
            Value.RunFin(input);

        public Either<Error, ReqState<IN>> RawRun(IN input) =>
            Value.RawRun(input);

        public ReqK<M, IN>.Full ToK<M>()
            where M : Monad<M> =>
            Value.ToK<M>();

        public static implicit operator Full(
            Req<IN, IN, IN, IN> value) =>
            new(value);

        public static implicit operator Req<IN, IN, IN, IN>(
            Full value) =>
            value.Value;
    }
}

public partial class Req<IN, OUT>
{
    /// <summary>
    /// Represents a full requirement pipeline whose local input/output
    /// coincide with the pipeline input/output.
    /// </summary>
    public readonly record struct Full(
        Req<IN, OUT, IN, OUT> Value)
    {
        public Fin<OUT> RunFin(IN input) =>
            Value.RunFin(input);

        public Either<Error, ReqState<OUT>> RawRun(IN input) =>
            Value.RawRun(input);

        public ReqK<M, IN, OUT>.Full ToK<M>()
            where M : Monad<M> =>
            Value.ToK<M>();

        public static implicit operator Full(
            Req<IN, OUT, IN, OUT> value) =>
            new(value);

        public static implicit operator Req<IN, OUT, IN, OUT>(
            Full value) =>
            value.Value;
    }
}
