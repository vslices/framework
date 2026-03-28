namespace LanguageExt;

public readonly record struct EffContext<RT, A, B>(Eff<RT, A> Ma, Func<A, B> Succ)
{
    public Eff<RT, B> Failure(Func<Error, B> fail) =>
        Ma.Match(Succ, fail);
}

public readonly record struct EffBindContext<RT, A, B>(Eff<RT, A> Ma, Func<A, Eff<RT, B>> Succ)
{
    public Eff<RT, B> FailureM(Func<Error, Eff<RT, B>> fail) =>
        Ma.BiBind(Succ, fail);

    public Eff<RT, B> Failure(Func<Error, B> fail) =>
        Ma.BiBind(Succ, e => Pure(fail(e)));

    public Eff<RT, B> Failure(Func<Error, Pure<B>> fail) =>
        Ma.BiBind(Succ, e => fail(e));
}

public static class VSlicesEffExtensions
{
    public static Eff<RT, B> BiBind<RT, A, B>(
        this Eff<RT, A> ma,
        Func<A, Eff<RT, B>> Succ,
        Func<Error, Eff<RT, B>> Fail) =>
        ma.Match(Succ, Fail).Flatten();

    public static EffContext<RT, A, B> Success<RT, A, B>(this Eff<RT, A> ma, Func<A, B> succ) =>
        new(ma, succ);

    public static EffBindContext<RT, A, B> SuccessM<RT, A, B>(this Eff<RT, A> ma, Func<A, Eff<RT, B>> succ) =>
        new(ma, succ);
}
