namespace VSlices.Traits;

public interface Kleisli<F, M> : Arrow<F>
    where F : Kleisli<F, M>
    where M : Monad<M>
{
    static abstract K<F, A, B> LiftK<A, B>(
        Func<A, K<M, B>> function);
}