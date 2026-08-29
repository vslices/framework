using VSlices.Monads;

namespace VSlices
{
    public static partial class VSlicesPrelude
    {
        public static Flow<RT, RQ, A> liftFlow<RT, RQ, A>(
            Func<RQ, Eff<A>> fa) =>
            Flow<RT, RQ>.Lift(fa);

        public static Flow<RT, RQ, A> liftFlow<RT, RQ, A>(
            Func<RQ, K<Eff, A>> fa) =>
            Flow<RT, RQ>.Lift(fa);
    }
}

namespace VSlices.Monads
{
    public partial class Flow<RT, RQ>
    {
        public static Flow<RT, RQ, A> Lift<A>(Func<RQ, Eff<A>> fa) =>
            new((_, req) => fa(req).RunIO());

        public static Flow<RT, RQ, A> Lift<A>(Func<RQ, K<Eff, A>> fa) =>
            new((_, req) => fa(req).RunIO());

        public static Flow<RT, RQ, A> Lift<A>(K<Eff, A> ma) =>
            new((_, _) => ma.RunIO());
    }
}
