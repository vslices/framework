namespace VSlices.Arrows;

public partial class ReqK<M, IN, OUT>
    where M : Monad<M>
{
    /// <summary>
    /// Represents a completed effectful requirement pipeline whose local
    /// input/output coincide with the pipeline input/output.
    /// </summary>
    public readonly record struct Completed(
        ReqK<M, IN, OUT, IN, OUT> Value)
    {
        public FinT<M, OUT> RunFinT(IN input) =>
            Value.RunFinT(input);

        public EitherT<Error, M, ReqState<OUT>> RawRun(IN input) =>
            Value.RawRun(input);

        public ReqK<M, IN, OUT>.Completed Apply<I2, O2>(
            ReqK<M, IN, OUT, I2, O2> rules,
            Func<OUT, I2> To) =>
            Value.Apply(rules, To);

        public static implicit operator Completed(
            ReqK<M, IN, OUT, IN, OUT> value) =>
            new(value);

        public static implicit operator ReqK<M, IN, OUT, IN, OUT>(
            Completed value) =>
            value.Value;
    }
}
