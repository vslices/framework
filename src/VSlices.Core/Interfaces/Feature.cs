using VSlices.Monads.Flow;

namespace VSlices.Core.Interfaces;

public interface Feature<F, RT, IN, OUT>
    where F : Feature<F, RT, IN, OUT>
{
    static abstract string Name { get; }

    static abstract Flow<RT, IN, OUT> Get();

    static virtual Fin<(OUT Result, FlowTrace Trace)> Run(
        IN input,
        RT runtime,
        EnvIO envIO) =>
        F.Get().Run(input, FlowTrace.Empty, runtime, envIO);

    static virtual (OUT Result, FlowTrace Trace) RunUnsafe(
        IN input,
        RT runtime,
        EnvIO envIO) =>
        F.Get().RunUnsafe(input, FlowTrace.Empty, runtime, envIO);

    static virtual Task<Fin<(OUT Result, FlowTrace Trace)>> RunAsync(
        IN input,
        RT runtime,
        EnvIO envIO) =>
        F.Get().RunAsync(input, FlowTrace.Empty, runtime, envIO);

    static virtual Task<(OUT Result, FlowTrace Trace)> RunUnsafeAsync(
        IN input,
        RT runtime,
        EnvIO envIO) =>
        F.Get().RunUnsafeAsync(input, FlowTrace.Empty, runtime, envIO);
}

public interface Feature<F, RT, IN> : Feature<F, RT, IN, Unit>
    where F : Feature<F, RT, IN>;
