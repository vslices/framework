using VSlices.Domain.Traits;
using VSlices.Domain.Traits.EventBuffer;

namespace VSlices.Domain.Interfaces;

public static class Repository
{
    public abstract class Base<RT, TRoot, TId> : IRepository<RT, TRoot, TId>
        where RT : Has<Eff<RT>, EventBufferIO>
        where TRoot : AggregateRoot<TRoot, TId>
        where TId : Identifier<TId>
    {
        public OptionT<Eff<RT>, TRoot> ReadOrOption(TId id) =>
            from root in ReadOrOptionCore(id)
            from _ in Buffer(root)
            select root;

        public Eff<RT, Seq<TRoot>> AddRange(Seq<TRoot> roots) =>
            from saved in AddRangeCore(roots)
            from _ in BufferRange(saved)
            select saved;

        public Eff<RT, Seq<TRoot>> UpdateRange(Seq<TRoot> roots) =>
            from saved in UpdateRangeCore(roots)
            from _ in BufferRange(saved)
            select saved;

        public Eff<RT, Seq<TRoot>> DeleteRange(Seq<TRoot> roots) =>
            from deleted in DeleteRangeCore(roots)
            from _ in BufferRange(deleted)
            select deleted;

        protected abstract OptionT<Eff<RT>, TRoot> ReadOrOptionCore(TId id);

        protected abstract Eff<RT, Seq<TRoot>> AddRangeCore(Seq<TRoot> roots);

        protected abstract Eff<RT, Seq<TRoot>> UpdateRangeCore(Seq<TRoot> roots);

        protected abstract Eff<RT, Seq<TRoot>> DeleteRangeCore(Seq<TRoot> roots);

        protected virtual Eff<RT, Unit> Buffer(TRoot root) =>
            EventBuffer<RT>.track(root);

        protected virtual Eff<RT, Unit> BufferRange(Seq<TRoot> roots) =>
            roots.TraverseM(Buffer).IgnoreF().As();
    }
}
