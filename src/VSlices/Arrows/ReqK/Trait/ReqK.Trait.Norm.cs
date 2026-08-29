using VSlices.Traits;
// ReSharper disable InconsistentNaming

namespace VSlices.Arrows;

public partial class ReqK
{

}

public partial class ReqK<M>
{
    
}

public partial class ReqK<M, IN>
    where M : Monad<M>
{

}

public partial class ReqK<M, IN, OUT>
    where M : Monad<M>
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="I"></typeparam>
    /// <typeparam name="I2"></typeparam>
    /// <typeparam name="O"></typeparam>
    /// <param name="m1"></param>
    /// <param name="m2"></param>
    /// <returns></returns>
    public static ReqK<M, IN, OUT, I, O> Compose<I, I2, O>(
        K<ReqK<M, IN, OUT>, I, I2> m1,
        K<ReqK<M, IN, OUT>, I2, O> m2) =>
        Category.Compose(m1, m2).AsBi();

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="I"></typeparam>
    /// <typeparam name="I2"></typeparam>
    /// <typeparam name="I3"></typeparam>
    /// <typeparam name="O"></typeparam>
    /// <param name="m1"></param>
    /// <param name="m2"></param>
    /// <param name="m3"></param>
    /// <returns></returns>
    public static ReqK<M, IN, OUT, I, O> Compose<I, I2, I3, O>(
        K<ReqK<M, IN, OUT>, I, I2> m1,
        K<ReqK<M, IN, OUT>, I2, I3> m2,
        K<ReqK<M, IN, OUT>, I3, O> m3) =>
        Category.Compose(m1, m2, m3).AsBi();

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="I"></typeparam>
    /// <typeparam name="I2"></typeparam>
    /// <typeparam name="I3"></typeparam>
    /// <typeparam name="I4"></typeparam>
    /// <typeparam name="O"></typeparam>
    /// <param name="m1"></param>
    /// <param name="m2"></param>
    /// <param name="m3"></param>
    /// <param name="m4"></param>
    /// <returns></returns>
    public static ReqK<M, IN, OUT, I, O> Compose<I, I2, I3, I4, O>(
        K<ReqK<M, IN, OUT>, I, I2> m1,
        K<ReqK<M, IN, OUT>, I2, I3> m2,
        K<ReqK<M, IN, OUT>, I3, I4> m3,
        K<ReqK<M, IN, OUT>, I4, O> m4) =>
        Category.Compose(m1, m2, m3, m4).AsBi();

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="I"></typeparam>
    /// <typeparam name="I2"></typeparam>
    /// <typeparam name="I3"></typeparam>
    /// <typeparam name="I4"></typeparam>
    /// <typeparam name="I5"></typeparam>
    /// <typeparam name="O"></typeparam>
    /// <param name="m1"></param>
    /// <param name="m2"></param>
    /// <param name="m3"></param>
    /// <param name="m4"></param>
    /// <param name="m5"></param>
    /// <returns></returns>
    public static ReqK<M, IN, OUT, I, O> Compose<I, I2, I3, I4, I5, O>(
        K<ReqK<M, IN, OUT>, I, I2> m1,
        K<ReqK<M, IN, OUT>, I2, I3> m2,
        K<ReqK<M, IN, OUT>, I3, I4> m3,
        K<ReqK<M, IN, OUT>, I4, I5> m4,
        K<ReqK<M, IN, OUT>, I5, O> m5) =>
        Category.Compose(m1, m2, m3, m4, m5).AsBi();

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="I"></typeparam>
    /// <typeparam name="I2"></typeparam>
    /// <typeparam name="I3"></typeparam>
    /// <typeparam name="I4"></typeparam>
    /// <typeparam name="I5"></typeparam>
    /// <typeparam name="I6"></typeparam>
    /// <typeparam name="O"></typeparam>
    /// <param name="m1"></param>
    /// <param name="m2"></param>
    /// <param name="m3"></param>
    /// <param name="m4"></param>
    /// <param name="m5"></param>
    /// <param name="m6"></param>
    /// <returns></returns>
    public static ReqK<M, IN, OUT, I, O> Compose<I, I2, I3, I4, I5, I6, O>(
        K<ReqK<M, IN, OUT>, I, I2> m1,
        K<ReqK<M, IN, OUT>, I2, I3> m2,
        K<ReqK<M, IN, OUT>, I3, I4> m3,
        K<ReqK<M, IN, OUT>, I4, I5> m4,
        K<ReqK<M, IN, OUT>, I5, I6> m5,
        K<ReqK<M, IN, OUT>, I6, O> m6) =>
        Category.Compose(m1, m2, m3, m4, m5, m6).AsBi();

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="I"></typeparam>
    /// <typeparam name="O"></typeparam>
    /// <param name="f"></param>
    /// <returns></returns>
    public static ReqK<M, IN, OUT, I, O> Lift<I, O>(Func<I, O> f) =>
        ReqK<M, IN, OUT, I>.Lift(f);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="I"></typeparam>
    /// <typeparam name="O"></typeparam>
    /// <param name="f"></param>
    /// <returns></returns>
    public static ReqK<M, IN, OUT, I, O> LiftK<I, O>(Func<I, K<M, O>> f) =>
        ReqK<M, IN, OUT, I>.LiftK(f);
    
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="I1"></typeparam>
    /// <typeparam name="I2"></typeparam>
    /// <typeparam name="O"></typeparam>
    /// <param name="ma"></param>
    /// <param name="fb"></param>
    /// <returns></returns>
    public static ReqK<M, IN, OUT, I1, O> Bind<I1, I2, O>(
        K<ReqK<M, IN, OUT>, I1, I2> ma,
        Func<I2, K<ReqK<M, IN, OUT>, I2, O>> fb) =>
        ArrowApply.Bind(ma, fb).AsBi();
}

public partial class ReqK<M, IN, OUT, I>
    where M : Monad<M>
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="I2"></typeparam>
    /// <typeparam name="O"></typeparam>
    /// <param name="m1"></param>
    /// <param name="m2"></param>
    /// <returns></returns>
    public static ReqK<M, IN, OUT, I, O> Compose<I2, O>(
        K<ReqK<M, IN, OUT>, I, I2> m1,
        K<ReqK<M, IN, OUT>, I2, O> m2) =>
        ReqK<M, IN, OUT>.Compose(m1, m2);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="I2"></typeparam>
    /// <typeparam name="O"></typeparam>
    /// <param name="m1"></param>
    /// <param name="m2"></param>
    /// <returns></returns>
    public static ReqK<M, IN, OUT, I, O> Compose<I2, O>(
        K<ReqK<M, IN, OUT>, I, I2> m1,
        ReqK<M, IN, OUT, I2, O> m2) =>
        ReqK<M, IN, OUT>.Compose(m1, m2);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="I2"></typeparam>
    /// <typeparam name="I3"></typeparam>
    /// <typeparam name="O"></typeparam>
    /// <param name="m1"></param>
    /// <param name="m2"></param>
    /// <param name="m3"></param>
    /// <returns></returns>
    public static ReqK<M, IN, OUT, I, O> Compose<I2, I3, O>(
        K<ReqK<M, IN, OUT>, I, I2> m1,
        K<ReqK<M, IN, OUT>, I2, I3> m2,
        K<ReqK<M, IN, OUT>, I3, O> m3) =>
        ReqK<M, IN, OUT>.Compose(m1, m2, m3);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="I2"></typeparam>
    /// <typeparam name="I3"></typeparam>
    /// <typeparam name="I4"></typeparam>
    /// <typeparam name="O"></typeparam>
    /// <param name="m1"></param>
    /// <param name="m2"></param>
    /// <param name="m3"></param>
    /// <param name="m4"></param>
    /// <returns></returns>
    public static ReqK<M, IN, OUT, I, O> Compose<I2, I3, I4, O>(
        K<ReqK<M, IN, OUT>, I, I2> m1,
        K<ReqK<M, IN, OUT>, I2, I3> m2,
        K<ReqK<M, IN, OUT>, I3, I4> m3,
        K<ReqK<M, IN, OUT>, I4, O> m4) =>
        ReqK<M, IN, OUT>.Compose(m1, m2, m3, m4);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="I2"></typeparam>
    /// <typeparam name="I3"></typeparam>
    /// <typeparam name="I4"></typeparam>
    /// <typeparam name="I5"></typeparam>
    /// <typeparam name="O"></typeparam>
    /// <param name="m1"></param>
    /// <param name="m2"></param>
    /// <param name="m3"></param>
    /// <param name="m4"></param>
    /// <param name="m5"></param>
    /// <returns></returns>
    public static ReqK<M, IN, OUT, I, O> Compose<I2, I3, I4, I5, O>(
        K<ReqK<M, IN, OUT>, I, I2> m1,
        K<ReqK<M, IN, OUT>, I2, I3> m2,
        K<ReqK<M, IN, OUT>, I3, I4> m3,
        K<ReqK<M, IN, OUT>, I4, I5> m4,
        K<ReqK<M, IN, OUT>, I5, O> m5) =>
        ReqK<M, IN, OUT>.Compose(m1, m2, m3, m4, m5);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="I2"></typeparam>
    /// <typeparam name="I3"></typeparam>
    /// <typeparam name="I4"></typeparam>
    /// <typeparam name="I5"></typeparam>
    /// <typeparam name="I6"></typeparam>
    /// <typeparam name="O"></typeparam>
    /// <param name="m1"></param>
    /// <param name="m2"></param>
    /// <param name="m3"></param>
    /// <param name="m4"></param>
    /// <param name="m5"></param>
    /// <param name="m6"></param>
    /// <returns></returns>
    public static ReqK<M, IN, OUT, I, O> Compose<I2, I3, I4, I5, I6, O>(
        K<ReqK<M, IN, OUT>, I, I2> m1,
        K<ReqK<M, IN, OUT>, I2, I3> m2,
        K<ReqK<M, IN, OUT>, I3, I4> m3,
        K<ReqK<M, IN, OUT>, I4, I5> m4,
        K<ReqK<M, IN, OUT>, I5, I6> m5,
        K<ReqK<M, IN, OUT>, I6, O> m6) =>
        ReqK<M, IN, OUT>.Compose(m1, m2, m3, m4, m5, m6);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="O"></typeparam>
    /// <param name="f"></param>
    /// <returns></returns>
    public static ReqK<M, IN, OUT, I, O> Lift<O>(Func<I, O> f) =>
        Arrow.Lift<ReqK<M, IN, OUT>, I, O>(f).AsBi();
    

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="O"></typeparam>
    /// <param name="f"></param>
    /// <returns></returns>
    public static ReqK<M, IN, OUT, I, O> LiftK<O>(Func<I, K<M, O>> f) =>
        Kleisli.LiftK<ReqK<M, IN, OUT>, M, I, O>(f).AsBi();
    
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="I2"></typeparam>
    /// <typeparam name="O"></typeparam>
    /// <param name="ma"></param>
    /// <param name="fb"></param>
    /// <returns></returns>
    public static ReqK<M, IN, OUT, I, O> Bind<I2, O>(
        K<ReqK<M, IN, OUT>, I, I2> ma,
        Func<I2, K<ReqK<M, IN, OUT>, I2, O>> fb) =>
        ReqK<M, IN, OUT>.Bind(ma, fb);
}
