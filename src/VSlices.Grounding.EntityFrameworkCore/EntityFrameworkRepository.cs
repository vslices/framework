using System.Linq.Expressions;
using EntityFrameworkCore.Functional;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using VSlices.Work;

namespace VSlices.Grounding.EntityFrameworkCore;

/// <summary>
/// Grounds <see cref="Repository{A}"/> on an Entity Framework Core projection.
/// </summary>
/// <typeparam name="TContext">Entity Framework Core context.</typeparam>
/// <typeparam name="A">Semantic value exposed by Work.</typeparam>
/// <typeparam name="TProjection">Persistence representation owned by this Grounding.</typeparam>
public abstract class EntityFrameworkRepository<TContext, A, TProjection>(TContext context)
    : Repository<A>
    where TContext : DbContext
    where TProjection : class
{
    protected TContext Context { get; } = context;

    protected DbSet<TProjection> Set => Context.Set<TProjection>();

    /// <summary>
    /// Materializes the persistence representation for a semantic value.
    /// </summary>
    protected abstract TProjection ToProjection(A value);

    /// <summary>
    /// Reconstructs the semantic value from its persistence representation.
    /// </summary>
    protected abstract A ToValue(TProjection projection);

    public IO<A> Create(A value) =>
        from projection in IO.lift(() => ToProjection(value))
        from entry in Context.AddIO(projection)
        from _ in Context.SaveChangesIO()
        select ToValue(entry.Entity);

    public IO<Seq<A>> Read() =>
        Set.AsNoTracking()
           .ToSeqIO()
           .Map(values => values.Map(ToValue));

    public IO<A> Update(A value) =>
        from projection in IO.lift(() => ToProjection(value))
        from entry in Context.UpdateIO(projection)
        from _ in Context.SaveChangesIO()
        select ToValue(entry.Entity);

    public IO<Unit> Delete(A value) =>
        from projection in IO.lift(() => ToProjection(value))
        from _ in Context.RemoveIO(projection)
        from __ in Context.SaveChangesIO()
        select unit;
}

/// <summary>
/// Direct Entity Framework Core grounding for semantic values that are themselves
/// persistence entities.
/// </summary>
public abstract class EntityFrameworkRepository<TContext, A>(TContext context)
    : EntityFrameworkRepository<TContext, A, A>(context)
    where TContext : DbContext
    where A : class
{
    protected sealed override A ToProjection(A value) => value;

    protected sealed override A ToValue(A projection) => projection;
}

/// <summary>
/// Grounds <see cref="Repository{A, ID}"/> on an Entity Framework Core projection
/// using a storage-native identity predicate.
/// </summary>
public abstract class EntityFrameworkRepository<TContext, A, ID, TProjection>(TContext context)
    : EntityFrameworkRepository<TContext, A, TProjection>(context),
      Repository<A, ID>
    where TContext : DbContext
    where TProjection : class
{
    /// <summary>
    /// Defines how the semantic identifier is represented by the persistence model.
    /// </summary>
    protected abstract Expression<Func<TProjection, bool>> ById(ID id);

    public OptionT<IO, A> Read(ID id) =>
        Set.AsNoTracking()
           .SingleOrNoneIO(ById(id))
           .Map(ToValue);

    public IO<bool> Any(ID id) =>
        Set.AsNoTracking()
           .AnyIO(ById(id));
}

/// <summary>
/// Direct Entity Framework Core grounding for identified semantic values that are
/// themselves persistence entities.
/// </summary>
public abstract class EntityFrameworkRepository<TContext, A, ID>(TContext context)
    : EntityFrameworkRepository<TContext, A, ID, A>(context)
    where TContext : DbContext
    where A : class;
