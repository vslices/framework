namespace VSlices.Traits;

/// <summary>
/// 
/// </summary>
public static class ArrowApply
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="F"></typeparam>
    /// <typeparam name="I"></typeparam>
    /// <typeparam name="O"></typeparam>
    /// <returns></returns>
    public static K<F, (K<F, I, O> Arrow, I Input), O> Apply<F, I, O>() 
        where F : ArrowApply<F> =>
        F.Apply<I, O>();

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="F"></typeparam>
    /// <typeparam name="I"></typeparam>
    /// <typeparam name="I2"></typeparam>
    /// <typeparam name="O"></typeparam>
    /// <param name="ma"></param>
    /// <param name="fb"></param>
    /// <returns></returns>
    public static K<F, I, O> Bind<F, I, I2, O>(K<F, I, I2> ma, Func<I2, K<F, I2, O>> fb) 
        where F : ArrowApply<F> =>
        F.Bind(ma, fb);

}