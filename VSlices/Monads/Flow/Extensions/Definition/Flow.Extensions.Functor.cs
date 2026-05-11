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
    public sealed partial class Flow<C, R, A>
    {
        public Flow<C, R, B> Map<B>(Func<A, B> fb) =>
            Flow<C, R>.Map(fb, this);

        public Flow<C, R, B> ConstMap<B>(B b) =>
            Flow<C, R>.ConstMap(b, this);

        public Flow<C, R, B> ConstMap<B>(Pure<B> pb) =>
            ConstMap(pb.Value);
    }
}
