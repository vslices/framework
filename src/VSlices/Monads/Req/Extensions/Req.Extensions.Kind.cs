using VSlices.Monads;

namespace VSlices;

/// <summary>
///
/// </summary>
public static class ReqKindExtensions
{
    extension<IN, A>(K<Req<IN>, A> value)
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public Req<IN, A> As() =>
            (Req<IN, A>)value;

        /// <summary>
        ///
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Fin<A> Onto(IN input) =>
            value.As().Onto(input);

        /// <summary>
        ///
        /// </summary>
        public ReqExecute<IN, A> RawRun =>
            value.As().RawRun;
    }

}
