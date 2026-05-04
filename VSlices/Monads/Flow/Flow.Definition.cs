namespace VSlices.Monads.Flow;

public sealed class FlowTrace(Seq<string> values) : Monoid<FlowTrace>
{
    public Seq<string> Steps { get; } = values;

    public FlowTrace Combine(FlowTrace rhs) => new(Steps.Concat(rhs.Steps));

    public static FlowTrace Empty { get; } = new(SeqEmpty.Default);


}

public partial class FlowOutcome<RT>
    : MonadUnliftIO<FlowOutcome<RT>>,
      Fallible<FlowOutcome<RT>>,
      Alternative<FlowOutcome<RT>>,
      MonoidK<FlowOutcome<RT>>,
      Final<FlowOutcome<RT>>,
      Readable<FlowOutcome<RT>, RT>
{
}

public partial class FlowOutcome<RT, OUT>(
    Eff<RT, (OUT Result, FlowTrace Trace)> effect)
    : K<FlowOutcome<RT>, OUT>
{
    public IO<(OUT Result, FlowTrace Trace)> RunIO(RT runtime) =>
        effect.RunIO(runtime);

    public Fin<(OUT Result, FlowTrace Trace)> Run(RT runtime, EnvIO env) =>
        effect.Run(runtime, env);

    public Task<Fin<(OUT Result, FlowTrace Trace)>> RunAsync(RT runtime, EnvIO env) =>
        effect.RunAsync(runtime, env);

    public (OUT Result, FlowTrace Trace) RunUnsafe(RT runtime, EnvIO env) =>
        effect.RunUnsafe(runtime, env);

    public async Task<(OUT Result, FlowTrace Trace)> RunUnsafeAsync(RT runtime, EnvIO env) =>
        await effect.RunUnsafeAsync(runtime, env);
}

public partial class Flow<RT, IN> 
    : MonadUnliftIO<Flow<RT, IN>>,
      Fallible<Flow<RT, IN>>,
      Alternative<Flow<RT, IN>>,
      MonoidK<Flow<RT, IN>>,
      Final<Flow<RT, IN>>,
      Readable<Flow<RT, IN>, RT>,
      Readable<Flow<RT, IN>, IN>,
      Writable<Flow<RT, IN>, FlowTrace>
{
}

public sealed partial class Flow<RT, IN, OUT>(
    Func<IN, FlowTrace, FlowOutcome<RT, OUT>> run) 
    : K<Flow<RT, IN>, OUT>
{
    public FlowOutcome<RT, OUT> RunFlow(IN input, FlowTrace trace) =>
        run(input, trace);

    public IO<(OUT Result, FlowTrace Trace)> RunIO(IN input, FlowTrace trace, RT runtime) =>
        RunFlow(input, trace).RunIO(runtime);

    public Fin<(OUT Result, FlowTrace Trace)> Run(IN input, FlowTrace trace, RT runtime, EnvIO env) =>
        RunFlow(input, trace).Run(runtime, env);

    public Task<Fin<(OUT Result, FlowTrace Trace)>> RunAsync(IN input, FlowTrace trace, RT runtime, EnvIO env) =>
        RunFlow(input, trace).RunAsync(runtime, env);

    public (OUT Result, FlowTrace Trace) RunUnsafe(IN input, FlowTrace trace, RT runtime, EnvIO env) =>
        RunFlow(input, trace).RunUnsafe(runtime, env);

    public async Task<(OUT Result, FlowTrace Trace)> RunUnsafeAsync(IN input, FlowTrace trace, RT runtime, EnvIO env) =>
        await RunFlow(input, trace).RunUnsafeAsync(runtime, env);
}

public static partial class FlowTraceExtensions
{
    public static K<F, A> IgnoreTrace<F, A>(this K<F, (A Result, FlowTrace Trace)> mat)
        where F : Functor<F> =>
        mat.Map(at => at.Result);
}
