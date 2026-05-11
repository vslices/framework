using VSlices.Monads;

namespace VSlices.Core.Interfaces;

public interface Feature<F, C, R, A>
    where F : Feature<F, C, R, A>
{
    static abstract string Name { get; }

    static abstract Flow<C, R, A> Get();

    static virtual Fin<A> Run(
        R input,
        C runtime,
        EnvIO envIO) =>
        F.Get().Run(runtime, input, envIO);

    static virtual A RunUnsafe(
        R input,
        C runtime,
        EnvIO envIO) =>
        F.Get().RunUnsafe(runtime, input, envIO);

    static virtual Task<Fin<A>> RunAsync(
        R input,
        C runtime,
        EnvIO envIO) =>
        F.Get().RunAsync(runtime, input, envIO);

    static virtual Task<A> RunUnsafeAsync(
        R input,
        C runtime,
        EnvIO envIO) =>
        F.Get().RunUnsafeAsync(runtime, input, envIO);
}

public interface Feature<F, C, R> : Feature<F, C, R, Unit>
    where F : Feature<F, C, R>;
