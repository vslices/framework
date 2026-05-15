using VSlices.Monads;

namespace VSlices.Application;

public interface Feature<F, RT, REQ, RES>
    where F : Feature<F, RT, REQ, RES>
{
    static abstract string Name { get; }

    static abstract Flow<RT, REQ, RES> Get();

    static virtual Fin<RES> Run(
        REQ input,
        RT runtime,
        EnvIO envIO) =>
        F.Get().Run(runtime, input, envIO);

    static virtual RES RunUnsafe(
        REQ input,
        RT runtime,
        EnvIO envIO) =>
        F.Get().RunUnsafe(runtime, input, envIO);

    static virtual Task<Fin<RES>> RunAsync(
        REQ input,
        RT runtime,
        EnvIO envIO) =>
        F.Get().RunAsync(runtime, input, envIO);

    static virtual Task<RES> RunUnsafeAsync(
        REQ input,
        RT runtime,
        EnvIO envIO) =>
        F.Get().RunUnsafeAsync(runtime, input, envIO);
}

public interface Feature<F, RT, REQ> : Feature<F, RT, REQ, Unit>
    where F : Feature<F, RT, REQ>;
