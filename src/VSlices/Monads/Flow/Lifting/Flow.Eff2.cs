using VSlices.Monads;

namespace VSlices
{
    public static partial class VSlicesPrelude
    {
        public static Flow<RT, RQ, A> liftFlow<RT, RQ, A>(
            Func<RQ, Eff<RT, A>> fa) =>
            Flow<RT, RQ>.Lift(fa);

        public static Flow<RT, RQ, A> liftFlow<RT, RQ, A>(
            Func<RQ, K<Eff<RT>, A>> fa) =>
            Flow<RT, RQ>.Lift(fa);
    }
}

namespace VSlices.Monads
{
    public partial class Flow<RT, RQ>
    {
        public static Flow<RT, RQ, A> Lift<A>(Func<RQ, Eff<RT, A>> fa) =>
            new((run, req) => fa(req).RunIO(run));

        public static Flow<RT, RQ, A> Lift<A>(Func<RQ, K<Eff<RT>, A>> fa) =>
            new((run, req) => fa(req).RunIO(run));

        public static Flow<RT, RQ, A> Lift<A>(K<Eff<RT>, A> ma) =>
            new((run, _) => ma.RunIO(run));
    }
}