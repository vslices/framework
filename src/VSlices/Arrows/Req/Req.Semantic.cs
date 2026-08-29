using LanguageExt;
using LanguageExt.Common;
using LanguageExt.Traits;
using VSlices.Traits;
using static LanguageExt.Prelude;

namespace VSlices.Arrows;

/// <summary>
/// 
/// </summary>
public partial class Req
{
    /// <summary>
    /// 
    /// </summary>
    public static readonly Pure<Unit> Ok = Pure(unit);
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="IN"></typeparam>
public partial class Req<IN>
{
    
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="IN"></typeparam>
/// <typeparam name="OUT"></typeparam>
public partial class Req<IN, OUT>
{
    /// <summary>
    /// 
    /// </summary>
    public static readonly Req<IN, OUT, Unit, IN> Input =
        Readable.ask<Req<IN, OUT, Unit>, IN>().As();
    
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="O"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Req<IN, OUT, Unit, O> Accept<O>(O value) =>
        Req<IN, OUT, Unit>.Accept(value);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    public static Req<IN, OUT, Unit, Unit> Write(Error error) =>
        Req<IN, OUT, Unit>.Write(error);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    public static Req<IN, OUT, Unit, Unit> Write(string e) => 
        Req<IN, OUT, Unit>.Write(e);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="I"></typeparam>
    /// <param name="f"></param>
    /// <returns></returns>
    public static Req<IN, OUT, I, bool> Check<I>(Func<I, bool> f) =>
        Req<IN, OUT, I>.Check(f); 

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="I"></typeparam>
    /// <param name="f"></param>
    /// <returns></returns>
    public static Req<IN, OUT, I, Unit> Prescribe<I>(Func<I, Error> f) =>
        Req<IN, OUT, I>.Prescribe(f);
        
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="I"></typeparam>
    /// <param name="fa"></param>
    /// <param name="Fail"></param>
    /// <returns></returns>
    public static Req<IN, OUT, I, I> Ensure<I>(
        Func<I, bool> fa, 
        Func<I, Error> Fail) =>
        Req<IN, OUT, I>.Ensure(fa, Fail);
        
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="I"></typeparam>
    /// <param name="fa"></param>
    /// <param name="Fail"></param>
    /// <returns></returns>
    public static Req<IN, OUT, I, I> Ensure<I>(
        Func<I, bool> fa, 
        Func<I, string> Fail) =>
        Req<IN, OUT, I>.Ensure(fa, Fail);
        
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="I"></typeparam>
    /// <param name="fa"></param>
    /// <param name="Fail"></param>
    /// <returns></returns>
    public static Req<IN, OUT, I, I> Ensure<I>(
        Func<I, bool> fa, 
        string Fail) =>
        Req<IN, OUT, I>.Ensure(fa, Fail);
                
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="I"></typeparam>
    /// <param name="fa"></param>
    /// <param name="Fail"></param>
    /// <returns></returns>
    public static Req<IN, OUT, I, I> Avoid<I>(
        Func<I, bool> fa, 
        Func<I, Error> Fail) =>
        Req<IN, OUT, I>.Avoid(fa, Fail);
                
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="I"></typeparam>
    /// <param name="fa"></param>
    /// <param name="Fail"></param>
    /// <returns></returns>
    public static Req<IN, OUT, I, I> Avoid<I>(
        Func<I, bool> fa, 
        Func<I, string> Fail) =>
        Req<IN, OUT, I>.Avoid(fa, Fail);
                
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="I"></typeparam>
    /// <param name="fa"></param>
    /// <param name="Fail"></param>
    /// <returns></returns>
    public static Req<IN, OUT, I, I> Avoid<I>(
        Func<I, bool> fa, 
        string Fail) =>
        Req<IN, OUT, I>.Avoid(fa, Fail);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="I"></typeparam>
    /// <typeparam name="O"></typeparam>
    /// <param name="f"></param>
    /// <returns></returns>
    public static Req<IN, OUT, I, O> Transform<I, O>(Func<I, O> f) =>
        new((_, previous) => 
            previous.Bind<ReqState<O>>(p => p.IsValid ? p.Map(f) : p.Error));

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="I2"></typeparam>
    /// <typeparam name="O2"></typeparam>
    /// <param name="rules"></param>
    /// <returns></returns>
    public static Req<IN, OUT, I2, O2> Adapt<I2, O2>(
        Req<I2, O2, I2, O2> rules) =>
        new((_, previous) =>
            previous.Bind(s => rules.RawRun(s.Value, previous)));
    
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="I"></typeparam>
    /// <typeparam name="I2"></typeparam>
    /// <typeparam name="O2"></typeparam>
    /// <param name="rules"></param>
    /// <param name="To"></param>
    /// <returns></returns>
    public static Req<IN, OUT, I, I> Apply<I, I2, O2>(
        Req<I2, O2, I2, O2> rules,
        Func<I, I2> To) =>
        Req<IN, OUT, I>.Identity
            .Bind(i => Compose(
                Lift(To),
                Adapt(rules),
                Lift<O2, I>(_ => i)));
    public static Req<IN, OUT, I, I> Apply<I, I2, O2>(
        Req<I2, O2>.Full rules,
        Func<I, I2> To) =>
        Apply(rules.Value, To);

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="I"></typeparam>
    /// <typeparam name="I2"></typeparam>
    /// <typeparam name="O2"></typeparam>
    /// <param name="rules"></param>
    /// <param name="To"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static Req<IN, OUT, I, I> ApplySeq<I, I2, O2>(
        Req<I2, O2, I2, O2> rules,
        Func<I, Seq<I2>> To) =>
        Req<IN, OUT, I>.Identity
            .Bind(i => To(i).Fold(
                Req<IN, OUT, I>.Identity, 
                (acc, item) => Compose(acc, Apply(rules, To: (I _) => item))));

    public static Req<IN, OUT, I, I> ApplySeq<I, I2, O2>(
        Req<I2, O2>.Full rules,
        Func<I, Seq<I2>> To) =>
        ApplySeq(rules.Value, To);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="I"></typeparam>
    /// <typeparam name="I2"></typeparam>
    /// <typeparam name="O2"></typeparam>
    /// <param name="rules"></param>
    /// <param name="To"></param>
    /// <returns></returns>
    // TODO: Mejorar para que use Option<T>
    public static Req<IN, OUT, I, I> ApplyOpt<I, I2, O2>(
        Req<I2, O2, I2, O2> rules,
        Func<I, Option<I2>> To) =>
        Req<IN, OUT, I>.Identity
            .Bind(i => To(i).Fold(
                Req<IN, OUT, I>.Identity,
                (acc, item) => Compose(acc, Apply(rules, To: (I _) => item))));

    public static Req<IN, OUT, I, I> ApplyOpt<I, I2, O2>(
        Req<I2, O2>.Full rules,
        Func<I, Option<I2>> To) =>
        ApplyOpt(rules.Value, To);
    
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="IN"></typeparam>
/// <typeparam name="OUT"></typeparam>
/// <typeparam name="I"></typeparam>
public partial class Req<IN, OUT, I>
{
    /// <summary>
    /// 
    /// </summary>
    public static readonly Req<IN, OUT, I, I> Identity =
        Category.Identity<Req<IN, OUT>, I>().AsBi();

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="O"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Req<IN, OUT, I, O> Accept<O>(O value) =>
        Arrow.Pure<Req<IN, OUT>, I, O>(value).AsBi();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    public static Req<IN, OUT, I, Unit> Write(Error error) =>
        Writable.tell<Req<IN, OUT, I>, Error>(error).As();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    public static Req<IN, OUT, I, Unit> Write(string e) => 
        Write(Error.New(e));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="f"></param>
    /// <returns></returns>
    public static Req<IN, OUT, I, bool> Check(Func<I, bool> f) =>
        Lift(f);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="f"></param>
    /// <returns></returns>
    public static Req<IN, OUT, I, Unit> Prescribe(Func<I, Error> f) =>
        Lift(f).Bind(Req<IN, OUT, Error>.Write);
        
    /// <summary>
    /// 
    /// </summary>
    /// <param name="fa"></param>
    /// <param name="Fail"></param>
    /// <returns></returns>
    public static Req<IN, OUT, I, I> Ensure(
        Func<I, bool> fa, 
        Func<I, Error> Fail) =>
        Identity.Bind(
            i => Compose(
                Identity, 
                fa(i) ? Req.Ok : Prescribe(Fail), 
                Req<IN, OUT, Unit>.Lift(_ => i)));
        
    /// <summary>
    /// 
    /// </summary>
    /// <param name="fa"></param>
    /// <param name="Fail"></param>
    /// <returns></returns>
    public static Req<IN, OUT, I, I> Ensure(
        Func<I, bool> fa, 
        Func<I, string> Fail) =>
        Ensure(fa, i => Error.New(Fail(i)));
        
    /// <summary>
    /// 
    /// </summary>
    /// <param name="fa"></param>
    /// <param name="Fail"></param>
    /// <returns></returns>
    public static Req<IN, OUT, I, I> Ensure(
        Func<I, bool> fa, 
        string Fail) =>
        Ensure(fa, _ => Fail);
                
    /// <summary>
    /// 
    /// </summary>
    /// <param name="fa"></param>
    /// <param name="Fail"></param>
    /// <returns></returns>
    public static Req<IN, OUT, I, I> Avoid(
        Func<I, bool> fa, 
        Func<I, Error> Fail) =>
        Ensure(i => !fa(i), Fail);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fa"></param>
    /// <param name="Fail"></param>
    /// <returns></returns>
    public static Req<IN, OUT, I, I> Avoid(
        Func<I, bool> fa, 
        Func<I, string> Fail) =>
        Avoid(fa, i => Error.New(Fail(i)));
                
    /// <summary>
    /// 
    /// </summary>
    /// <param name="fa"></param>
    /// <param name="Fail"></param>
    /// <returns></returns>
    public static Req<IN, OUT, I, I> Avoid(
        Func<I, bool> fa, 
        string Fail) =>
        Avoid(fa, i => Fail);
}