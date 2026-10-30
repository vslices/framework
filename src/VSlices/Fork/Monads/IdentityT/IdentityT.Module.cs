using ForkExt.Traits;

namespace ForkExt;

public class IdentityT
{
    public static IdentityT<M, A> Pure<M, A>(A value) 
        where M : Monad<M> =>
        new (M.Pure(value));

    public static IdentityT<M, A> lift<M, A>(K<M, A> value) 
        where M : Monad<M> =>
        new (value);
}
