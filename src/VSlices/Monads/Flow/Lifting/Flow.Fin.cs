using VSlices.Monads;

namespace VSlices
{
    public static partial class VSlicesPrelude
    {
        public static Flow<RT, RQ, A> liftFlow<RT, RQ, A>(
            Func<RQ, Fin<A>> fa) =>
            Flow<RT, RQ>.Lift(fa);

        public static Flow<RT, RQ, A> liftFlow<RT, RQ, A>(
            Func<RQ, K<Fin, A>> fa) =>
            Flow<RT, RQ>.Lift(fa);
    }
}

namespace VSlices.Monads
{
    public partial class Flow<RT, RQ>
    {
        public static Flow<RT, RQ, A> Lift<A>(Func<RQ, Fin<A>> fa) =>
            new((_, req) => fa(req).Match(Succ: IO.pure, Fail: IO.fail<A>));

        public static Flow<RT, RQ, A> Lift<A>(Func<RQ, K<Fin, A>> fa) =>
            new((_, req) => fa(req).As().Match(Succ: IO.pure, Fail: IO.fail<A>));

        public static Flow<RT, RQ, A> Lift<A>(K<Fin, A> ma) =>
            new((_, _) => ma.As().Match(Succ: IO.pure, Fail: IO.fail<A>));
    }
}
