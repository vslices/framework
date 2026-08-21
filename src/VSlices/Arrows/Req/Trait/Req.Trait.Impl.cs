using VSlices.Traits;
using LanguageExt;
using LanguageExt.Traits;
using LanguageExt.Common;

namespace VSlices.Monads;

public partial class Req<IN, OUT> :
    ArrowApply<Req<IN, OUT>>
{
    static K<Req<IN, OUT>, I1, O> Category<Req<IN, OUT>>.Compose<I1, I2, O>(
        K<Req<IN, OUT>, I1, I2> first, K<Req<IN, OUT>, I2, O> second) =>
        new Req<IN, OUT, I1, O>(
            (input, previous) => from next in previous.Bind(p => first.RawRunBi(input, p))
                                 from current in second.RawRunBi(input, next)
                                 select current);

    static K<Req<IN, OUT>, A, A> Category<Req<IN, OUT>>.Identity<A>() =>
        new Req<IN, OUT, A, A>(
            (_, previous) => previous);

    static K<Req<IN, OUT>, A, B> Arrow<Req<IN, OUT>>.Lift<A, B>(Func<A, B> fb) =>
        new Req<IN, OUT, A, B>(
            (_, previous) => previous.Map(n => n.Map(fb)));

    static K<Req<IN, OUT>, (A, C), (B, C)> Arrow<Req<IN, OUT>>.First<A, B, C>(K<Req<IN, OUT>, A, B> arrow) =>
        new Req<IN, OUT, (A, C), (B, C)>(
            (input, previous) => previous.Bind(p =>
            {
                var (aValue, cValue) = p.Value;

                return arrow.RawRunBi(input, p.Map(_ => aValue))
                    .Map(n => n.Map(s => (s, cValue)));
            }));

    static K<Req<IN, OUT>, (K<Req<IN, OUT>, I, O> Arrow, I Input), O> ArrowApply<Req<IN, OUT>>.Apply<I, O>() =>
        new Req<IN, OUT, (K<Req<IN, OUT>, I, O> Arrow, I Input), O>(
            (inp, previous) => previous.Bind(pair =>
            {
                var (arrow, input) = pair.Value;

                return arrow.RawRunBi(inp, pair.Map(_ => input));
            }));
}


public partial class Req<IN, OUT, I> :
    Readable<Req<IN, OUT, I>, IN>,
    Writable<Req<IN, OUT, I>, Error>
{
    static K<Req<IN, OUT, I>, A> Readable<Req<IN, OUT, I>, IN>.Asks<A>(
          Func<IN, A> f) =>
          new Req<IN, OUT, I, A>(
              (input, previous) => previous.Map(p => p.Map(_ => f(input))));

    static K<Req<IN, OUT, I>, A> Readable<Req<IN, OUT, I>, IN>.Local<A>(
        Func<IN, IN> f,
        K<Req<IN, OUT, I>, A> ma) =>
        new Req<IN, OUT, I, A>(
            (input, previous) => previous.Bind(p => ma.RawRun(f(input), p)));

    static K<Req<IN, OUT, I>, (A Value, Error Output)> Writable<Req<IN, OUT, I>, Error>.Listen<A>(
        K<Req<IN, OUT, I>, A> ma) =>
        new Req<IN, OUT, I, (A Value, Error Output)>(
            (input, previous) => ma.RawRun(input, previous)
                                   .Map(s => s.Express()));

    static K<Req<IN, OUT, I>, A> Writable<Req<IN, OUT, I>, Error>.Pass<A>(
        K<Req<IN, OUT, I>, (A Value, Func<Error, Error> Function)> action) =>
        new Req<IN, OUT, I, A>(
            (input, previous) => action.RawRun(input, previous)
                                       .Map(n => n.Bind(s => ReqState.New(s.Value, s.Function(n.Error)))));

    static K<Req<IN, OUT, I>, Unit> Writable<Req<IN, OUT, I>, Error>.Tell(
        Error item) =>
        new Req<IN, OUT, I, Unit>((_, previous) =>
            previous.Map(p => p.Bind(_ => ReqState.Unit(p.Error + item))));
}
