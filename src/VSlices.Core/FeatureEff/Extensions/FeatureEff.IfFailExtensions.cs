using VSlices.Core.Errors;

namespace VSlices.Core;

public sealed partial record FeatureEff<RT, A>
{
    public FeatureEff<RT, A> IfFail(Func<FeatureError, A> f) =>
        Effect.IfLeft(f)
              .ToFeatureEff();

    public FeatureEff<RT, A> IfFail(
        Func<FeatureError, A> Fail,
        Func<Error, A> Except) =>
        Effect.IfLeft(Fail).As()
              .IfFail(Except)
              .ToFeatureEff();

    public FeatureEff<RT, A> IfFailM(Func<FeatureError, FeatureEff<RT, A>> f) =>
        Effect.Run().As()
              .SuccessM(r => r.Right(Pure).Left(f).RunEff())
              .FailureM(e => Prelude.Fail(e))
              .ToFeatureEff();

    public FeatureEff<RT, A> IfFailM(
        Func<FeatureError, FeatureEff<RT, A>> Fail,
        Func<Error, FeatureEff<RT, A>> Except) =>
        Effect.Run().As()
              .SuccessM(r => r.Right(Pure).Left(Fail).RunEff())
              .FailureM(e => Except(e).RunEff())
              .ToFeatureEff();
}
