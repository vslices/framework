using VSlices.Domain.Environments.EventBuffer;
using VSlices.Domain.Traits;

namespace VSlices.Domain.Interfaces;

public static class Repository
{
    public abstract class Base<M, RT, TRoot, TId> : IRepository<M, TRoot, TId>
        where M : MonadIO<M>
        where RT : Has<M, EventBufferIO>
        where TRoot : AggregateRoot<TRoot, TId>
        where TId : Identifier<TId>
    {
        public OptionT<M, TRoot> ReadOrOption(TId id) =>
            from root in ReadOrOptionCore(id)
            from _ in Buffer(root)
            select root;

        public K<M, Seq<TRoot>> AddRange(Seq<TRoot> roots) =>
            from saved in AddRangeCore(roots)
            from _ in BufferRange(saved)
            select saved;

        public K<M, Seq<TRoot>> UpdateRange(Seq<TRoot> roots) =>
            from saved in UpdateRangeCore(roots)
            from _ in BufferRange(saved)
            select saved;

        public K<M, Seq<TRoot>> DeleteRange(Seq<TRoot> roots) =>
            from deleted in DeleteRangeCore(roots)
            from _ in BufferRange(deleted)
            select deleted;

        protected abstract OptionT<M, TRoot> ReadOrOptionCore(TId id);

        protected abstract K<M, Seq<TRoot>> AddRangeCore(Seq<TRoot> roots);

        protected abstract K<M, Seq<TRoot>> UpdateRangeCore(Seq<TRoot> roots);

        protected abstract K<M, Seq<TRoot>> DeleteRangeCore(Seq<TRoot> roots);

        protected virtual K<M, Unit> Buffer(TRoot root) =>
            EventBufferEnv<M, RT>.track(root);

        protected virtual K<M, Unit> BufferRange(Seq<TRoot> roots) =>
            roots.TraverseM(Buffer).IgnoreF();
    }
}
