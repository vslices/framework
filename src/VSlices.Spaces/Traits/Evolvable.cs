using LanguageExt;

namespace VSlices.Spaces;

/// <summary>
/// Describes a point whose accepted state can evolve without mutating the source instance.
/// </summary>
/// <typeparam name="SELF">The semantic point type.</typeparam>
/// <typeparam name="STATE">The shape of the point's current state.</typeparam>
public interface Evolvable<SELF, STATE>
    where SELF : Evolvable<SELF, STATE>
{
    /// <summary>
    /// Gets the state currently accepted by this point.
    /// </summary>
    STATE CurrentState { get; }

    /// <summary>
    /// Validates a proposed transition from an accepted state to a new state
    /// and, when admissible, materializes a new point without mutating the source.
    /// The semantic owner decides which changes are allowed.
    /// </summary>
    static abstract Fin<SELF> Evolve(STATE old, STATE @new);

    /// <summary>
    /// Proposes a transformation over the current state. Both the previous
    /// state and the candidate state are passed to the semantic transition rule.
    /// </summary>
    Fin<SELF> Update(Func<STATE, STATE> update) =>
        SELF.Evolve(CurrentState, update(CurrentState));
}

public static class Evolvable
{
    public static Fin<SELF> update<SELF, STATE>(Evolvable<SELF, STATE> evolvable, Func<STATE, STATE> update)
        where SELF : Evolvable<SELF, STATE> =>
        evolvable.Update(update);
}

public static class EvolvableExtensions
{
    public static Fin<SELF> Update<SELF, STATE>(this Evolvable<SELF, STATE> evolvable, Func<STATE, STATE> update)
        where SELF : Evolvable<SELF, STATE> =>
        Evolvable.update(evolvable, update);
}
