using VSlices.Monads;

namespace VSlices;

public static class ReqKindExtensions
{
    extension<IN, A>(K<Req<IN>, A> value)
    {
        public Req<IN, A> As() =>
            (Req<IN, A>)value;

        public Fin<A> Run(IN input) =>
            value.As().Run(input);

        public ReqExecute<IN, A> RawRun =>
            value.As().RawRun;
    }

}
