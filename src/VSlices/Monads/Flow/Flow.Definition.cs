namespace VSlices.Monads;

public sealed partial class Flow<RT, REQ, RES>(
    Func<RT, REQ, IO<RES>> run)
    : K<Flow<RT, REQ>, RES>
    where RES : notnull
{
    public IO<RES> RunFlow(RT state, REQ request) =>
        run(state, request);

    public Eff<RT, RES> RunEff(REQ input) =>
        Eff<RT, RES>.LiftIO(state => run(state, input));

    public static implicit operator Flow<RT, REQ, RES>(Pure<RES> a) =>
        Flow<RT, REQ>.Pure(a);

    public static implicit operator Flow<RT, REQ, RES>(Fail<Error> a) =>
        Flow<RT, REQ>.Fail<RES>(a);

}
