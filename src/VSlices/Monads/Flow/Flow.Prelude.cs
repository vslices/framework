using VSlices.Monads;

namespace VSlices;

public static partial class VSlicesPrelude
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="RT"></typeparam>
    /// <typeparam name="REQ"></typeparam>
    /// <typeparam name="RES"></typeparam>
    /// <param name="f"></param>
    /// <returns></returns>
    public static Flow<RT, REQ, RES> liftFlow<RT, REQ, RES>(
        Func<RT, REQ, IO<RES>> f) =>
        Flow<RT, REQ>.LiftIO(f);

    
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="RT"></typeparam>
    /// <typeparam name="REQ"></typeparam>
    /// <typeparam name="RES"></typeparam>
    /// <param name="f"></param>
    /// <returns></returns>
    public static Flow<RT, REQ, RES> liftFlow<RT, REQ, RES>(
        Func<RT, REQ, EnvIO, Task<RES>> f) =>
        Flow<RT, REQ>.LiftIO(f);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="RT"></typeparam>
    /// <typeparam name="REQ"></typeparam>
    /// <typeparam name="RES"></typeparam>
    /// <param name="f"></param>
    /// <returns></returns>
    public static Flow<RT, REQ, RES> liftFlow<RT, REQ, RES>(
        Func<RT, REQ, RES> f) =>
        Flow<RT, REQ>.Lift(f);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="RT"></typeparam>
    /// <typeparam name="REQ"></typeparam>
    /// <typeparam name="RES"></typeparam>
    /// <param name="f"></param>
    /// <returns></returns>
    public static Flow<RT, REQ, RES> liftFlow<RT, REQ, RES>(
        Func<RT, REQ, FinT<IO, RES>> f) =>
        liftFlow((RT c, REQ r) =>
            f(c, r).Run().As().Bind<RES>(ma => ma.Match(IO.pure, IO.fail<RES>)));
    
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="RT"></typeparam>
    /// <typeparam name="REQ"></typeparam>
    /// <typeparam name="RES"></typeparam>
    /// <param name="f"></param>
    /// <returns></returns>
    public static Flow<RT, REQ, RES> liftFlow<RT, REQ, RES>(
        Func<RT, REQ, EnvIO, Task<Fin<RES>>> f) =>
        liftFlow((RT c, REQ r) => FinT.lift(IO.liftAsync(e => f(c, r, e))));
    
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="RT"></typeparam>
    /// <typeparam name="REQ"></typeparam>
    /// <typeparam name="RES"></typeparam>
    /// <param name="f"></param>
    /// <returns></returns>
    public static Flow<RT, REQ, RES> liftFlow<RT, REQ, RES>(
        Func<RT, REQ, Fin<RES>> f) =>
        liftFlow((RT c, REQ r) => FinT.lift<IO, RES>(f(c, r)));

}
