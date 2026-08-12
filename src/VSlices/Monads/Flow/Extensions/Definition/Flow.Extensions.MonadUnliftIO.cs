// Resharper disable CheckNamespace
using VSlices.Monads;

namespace VSlices;

public static partial class FlowExtensions
{
    extension<C, R, A>(K<Flow<C, R>, A> ma)
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public Flow<C, R, IO<A>> ToIO() =>
            +MonadUnliftIO.toIO(ma);

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="B"></typeparam>
        /// <param name="f"></param>
        /// <returns></returns>
        public Flow<C, R, B> MapIO<B>(Func<IO<A>, IO<B>> f) =>
            +MonadUnliftIO.mapIO(f, ma);
    }
}
