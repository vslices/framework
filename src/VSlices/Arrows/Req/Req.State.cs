namespace VSlices.Monads;

/// <summary>
/// 
/// </summary>
public static class ReqState
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="O"></typeparam>
    /// <param name="v"></param>
    /// <returns></returns>
    public static ReqState<O> New<O>(O v) =>
        New(v, Error.Empty);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="O"></typeparam>
    /// <param name="v"></param>
    /// <param name="e"></param>
    /// <returns></returns>
    public static ReqState<O> New<O>(O v, Error e) =>
        new(v, e);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    public static ReqState<Unit> Unit(Error e) =>
        new(unit, e);
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="O"></typeparam>
/// <param name="Value"></param>
/// <param name="Error"></param>
public readonly record struct ReqState<O>(O Value, Error Error)
{
    /// <summary>
    /// 
    /// </summary>
    public bool IsValid => Error.IsEmpty;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="O2"></typeparam>
    /// <param name="f"></param>
    /// <returns></returns>
    public ReqState<O2> Map<O2>(Func<O, O2> f) =>
        new(f(Value), Error);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="f"></param>
    /// <returns></returns>
    public ReqState<O> MapError(Func<Error, Error> f) =>
        this with { Error = f(Error) };

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="O2"></typeparam>
    /// <param name="f"></param>
    /// <returns></returns>
    public ReqState<O2> Bind<O2>(Func<O, ReqState<O2>> f) =>
        f(Value);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public ReqState<(O, Error)> Express()
    {
        var response = this;
        return Map(v => (v, response.Error));
    }
}