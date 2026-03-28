using System;
using System.Collections.Generic;
using System.Text;
using VSlices.Core.Errors;

namespace VSlices.Core;

public sealed partial record FeatureEff<RT, A>
{
    public FeatureEff<RT, C> SelectMany<B, C>(
        Func<A, FeatureEff<RT, B>> bind, 
        Func<A, B, C> projection) =>
        Bind(a => bind(a).Map(b => projection(a, b)));

    public FeatureEff<RT, C> SelectMany<B, C>(
        Func<A, K<FeatureEff<RT>, B>> bind,
        Func<A, B, C> projection) =>
        SelectMany(a => bind(a).As(), projection);

    public FeatureEff<RT, C> SelectMany<B, C>(
        Func<A, IO<B>> bind,
        Func<A, B, C> projection) =>
        SelectMany(a => bind(a).ToFeatureEff<RT, B>(), projection);

    public FeatureEff<RT, C> SelectMany<B, C>(
        Func<A, K<IO, B>> bind,
        Func<A, B, C> projection) =>
        SelectMany(a => bind(a).As(), projection);

    public FeatureEff<RT, C> SelectMany<B, C>(
        Func<A, Eff<RT, B>> bind,
        Func<A, B, C> projection) =>
        SelectMany(a => bind(a).ToFeatureEff(), projection);

    public FeatureEff<RT, C> SelectMany<B, C>(
        Func<A, K<Eff<RT>, B>> bind,
        Func<A, B, C> projection) =>
        SelectMany(a => bind(a).As(), projection);

    public FeatureEff<RT, C> SelectMany<B, C>(
        Func<A, Eff<B>> bind,
        Func<A, B, C> projection) =>
        SelectMany(a => bind(a).ToFeatureEff<RT, B>(), projection);

    public FeatureEff<RT, C> SelectMany<B, C>(
        Func<A, K<Eff, B>> bind,
        Func<A, B, C> projection) =>
        SelectMany((a) => bind(a).As(), projection);

    public FeatureEff<RT, C> SelectMany<B, C>(
        Func<A, Pure<B>> bind,
        Func<A, B, C> projection) =>
        SelectMany(FeatureEff<RT, B> (a) => bind(a), projection);

    public FeatureEff<RT, C> SelectMany<B, C>(
        Func<A, Fail<Exceptional>> bind,
        Func<A, B, C> projection) =>
        SelectMany(FeatureEff<RT, B> (a) => bind(a), projection);

    public FeatureEff<RT, C> SelectMany<B, C>(
        Func<A, Fail<FeatureError>> bind,
        Func<A, B, C> projection) =>
        SelectMany(FeatureEff<RT, B> (a) => bind(a), projection);

    public FeatureEff<RT, C> SelectMany<C>(
        Func<A, Guard<Error, Unit>> bind, 
        Func<A, Unit, C> project) =>
        from x in this
        from r in bind(x) switch
        {
            { Flag: true } => FeatureEff<RT, Unit>.Pure(unit),
            var g => g.OnFalse() switch
            {
                Exceptional e => FeatureEff<RT, Unit>.Fail(e),
                FeatureError fe => FeatureEff<RT, Unit>.Fail(fe),
                Error e => Eff<RT, Unit>.Fail(e).ToFeatureEff()
            }
        }
        select project(x, unit);

    public FeatureEff<RT, C> SelectMany<C>(
        Func<A, Guard<Fail<Error>, Unit>> bind, Func<A, Unit, C> project) =>
        from x in this
        from r in bind(x) switch
        {
            { Flag: true } => FeatureEff<RT, Unit>.Pure(unit),
            var g => g.OnFalse().Value switch
            {
                Exceptional e => FeatureEff<RT, Unit>.Fail(e),
                FeatureError fe => FeatureEff<RT, Unit>.Fail(fe),
                Error e => Eff<RT, Unit>.Fail(e).ToFeatureEff()
            }
        }
        select project(x, unit);
}
