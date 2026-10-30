using ForkExt.Traits;

namespace ForkExt;

public static class IdentityTExt
{
    extension<M, A>(K<IdentityT<M>, A> ma) where M : Monad<M>
    {
        public IdentityT<M, A> As() =>
            (IdentityT<M, A>)ma;

        public K<M, A> Run() =>
            ((IdentityT<M, A>)ma).Value;
    }
}
