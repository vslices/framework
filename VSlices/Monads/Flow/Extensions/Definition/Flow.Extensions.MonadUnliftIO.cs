// Resharper disable CheckNamespace
using VSlices.Monads;

namespace VSlices;

public static partial class FlowExtensions
{
    extension<C, R, A>(K<Flow<C, R>, A> ma)
    {
        public Flow<C, R, IO<A>> ToIO() =>
            +MonadUnliftIO.toIO(ma);

        public Flow<C, R, B> MapIO<B>(Func<IO<A>, IO<B>> f) =>
            +MonadUnliftIO.mapIO(f, ma);
    }
}
