namespace VSlices.Core;

public static partial class FeatureEffExtensions
{
    extension<RT, A>(K<FeatureEff<RT>, A>)
    {
        public static FeatureEff<RT, A> operator +(K<FeatureEff<RT>, A> ma) =>
            ma.As();
    }
}
