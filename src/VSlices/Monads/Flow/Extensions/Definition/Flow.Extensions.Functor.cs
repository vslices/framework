// Resharper disable CheckNamespace
using VSlices.Monads;

namespace VSlices
{
    public static partial class FlowExtensions
    {
        extension<C, R, A>(K<Flow<C, R>, A> ma)
        {
            public Flow<C, R, B> Map<B>(Func<A, B> fb) =>
                ma.As().Map(fb);

            public Flow<C, R, B> ConstMap<B>(B b) =>
                ma.As().ConstMap(b);

            public Flow<C, R, B> ConstMap<B>(Pure<B> pb) =>
                ma.As().ConstMap(pb);

            public Flow<C, R, Unit> Ignore() =>
                ma.As().ConstMap(unit);

        }
    }
}

namespace VSlices.Monads
{
    public sealed partial class Flow<RT, REQ, RES>
    {
        public Flow<RT, REQ, B> Map<B>(Func<RES, B> fb) =>
            Flow<RT, REQ>.Map(fb, this);

        public Flow<RT, REQ, B> ConstMap<B>(B b) =>
            Flow<RT, REQ>.ConstMap(b, this);

        public Flow<RT, REQ, B> ConstMap<B>(Pure<B> pb) =>
            ConstMap(pb.Value);
    }
}
