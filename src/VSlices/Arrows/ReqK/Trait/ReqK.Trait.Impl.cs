using System;
using System.Collections.Generic;
using System.Text;
using VSlices.Traits;

namespace VSlices.Arrows;

public partial class ReqK<M, IN, OUT> :
    ArrowApply<ReqK<M, IN, OUT>>,
    Kleisli<ReqK<M, IN, OUT>, M>
    where M : Monad<M>
{
    static K<ReqK<M, IN, OUT>, A, A> Category<ReqK<M, IN, OUT>>.Identity<A>() =>
        new ReqK<M, IN, OUT, A, A>(
            (_, previous) =>
                EitherT.lift<Error, M, ReqState<A>>(previous));

    static K<ReqK<M, IN, OUT>, I1, O> Category<ReqK<M, IN, OUT>>.Compose<I1, I2, O>(
        K<ReqK<M, IN, OUT>, I1, I2> first, 
        K<ReqK<M, IN, OUT>, I2, O> second) =>
        new ReqK<M, IN, OUT, I1, O>(
            (input, previous) =>
                first.RawRunBi(input, previous)
                     .Bind(next => second.RawRunBi(input, next)));

    static K<ReqK<M, IN, OUT>, I, O> Arrow<ReqK<M, IN, OUT>>.Lift<I, O>(Func<I, O> function) =>
        new ReqK<M, IN, OUT, I, O>(
            (_, previous) => previous.Map(state => state.Map(function)));

    static K<ReqK<M, IN, OUT>, (I, X), (O, X)> Arrow<ReqK<M, IN, OUT>>.First<I, O, X>(K<ReqK<M, IN, OUT>, I, O> arrow) =>
        new ReqK<M, IN, OUT, (I, X), (O, X)>(
            (input, previous) =>
                previous.Match(
                    Left: EitherT.Left<Error, M, ReqState<(O, X)>>,
                    Right: state => arrow
                        .RawRunBi(input, state.Map(pair => pair.Item1))
                        .Map(next => next.Map(output => (output, state.Value.Item2)))));

    static K<ReqK<M, IN, OUT>, (K<ReqK<M, IN, OUT>, I, O> Arrow, I Input), O> ArrowApply<ReqK<M, IN, OUT>>.Apply<I, O>() =>
        new ReqK<M, IN, OUT, (K<ReqK<M, IN, OUT>, I, O> Arrow, I Input), O>(
            (input, previous) =>
                previous.Match(
                    Left: EitherT.Left<Error, M, ReqState<O>>,
                    Right: s => s.Value.Arrow.RawRunBi(input, s.Map(p => p.Input))));

    static K<ReqK<M, IN, OUT>, A, B> Kleisli<ReqK<M, IN, OUT>, M>.LiftK<A, B>(Func<A, K<M, B>> function) =>
        new ReqK<M, IN, OUT, A, B>(
            (_, previous) =>
                previous.Match(
                    Left: EitherT.Left<Error, M, ReqState<B>>,
                    Right: state =>
                        EitherT.lift<Error, M, ReqState<B>>(
                            function(state.Value).Map(value => state.Map(_ => value)))));
}

public partial class ReqK<M, IN, OUT, I> :
    Readable<ReqK<M, IN, OUT, I>, IN>,
    Writable<ReqK<M, IN, OUT, I>, Error>
    where M : Monad<M>
{
    static K<ReqK<M, IN, OUT, I>, A> Readable<ReqK<M, IN, OUT, I>, IN>.Asks<A>(Func<IN, A> f) =>
        new ReqK<M, IN, OUT, I, A>(
            (input, previous) =>
                previous.Map(state =>
                    state.Map(_ => f(input))));

    static K<ReqK<M, IN, OUT, I>, A> Readable<ReqK<M, IN, OUT, I>, IN>.Local<A>(
        Func<IN, IN> f, 
        K<ReqK<M, IN, OUT, I>, A> ma) =>
        new ReqK<M, IN, OUT, I, A>(
            (input, previous) => ma.RawRun(f(input), previous));

    static K<ReqK<M, IN, OUT, I>, Unit> Writable<ReqK<M, IN, OUT, I>, Error>.Tell(
        Error item) =>
        new ReqK<M, IN, OUT, I, Unit>(
            (_, previous) => previous
                .Map(state => state.Map(_ => unit).MapError(error => error + item)));

    static K<ReqK<M, IN, OUT, I>, (A Value, Error Output)> Writable<ReqK<M, IN, OUT, I>, Error>.Listen<A>(
        K<ReqK<M, IN, OUT, I>, A> ma) =>
        new ReqK<M, IN, OUT, I, (A Value, Error Output)>(
            (input, previous) => ma.RawRun(input, previous)
                .Map(state => state.Map(value => (value, state.Error))));

    static K<ReqK<M, IN, OUT, I>, A> Writable<ReqK<M, IN, OUT, I>, Error>.Pass<A>(
        K<ReqK<M, IN, OUT, I>, (A Value, Func<Error, Error> Function)> action) =>
        new ReqK<M, IN, OUT, I, A>(
            (input, previous) =>
                action.RawRun(input, previous)
                      .Map(state =>
                      {
                          var (value, function) = state.Value;

                          return ReqState.New(
                              value,
                              function(state.Error));
                      }));
}
