namespace VSlices.Arrows;

public partial class Req<IN, OUT>
{
    /// <summary>
    /// Represents a completed requirement pipeline whose local input/output
    /// coincide with the pipeline input/output.
    /// </summary>
    public readonly record struct Completed(
        Req<IN, OUT, IN, OUT> Value)
    {
        public Fin<OUT> RunFin(IN input) =>
            Value.RunFin(input);

        public Either<Error, ReqState<OUT>> RawRun(IN input) =>
            Value.RawRun(input);

        public ReqK<M, IN, OUT>.Completed ToK<M>()
            where M : Monad<M> =>
            Value.ToK<M>();

        public static implicit operator Completed(
            Req<IN, OUT, IN, OUT> value) =>
            new(value);

        public static implicit operator Req<IN, OUT, IN, OUT>(
            Completed value) =>
            value.Value;
    }
}
