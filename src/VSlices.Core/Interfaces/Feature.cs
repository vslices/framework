namespace VSlices.Core.Interfaces;

public interface Feature<RT, TIn, TOut>
{
    static abstract string Name { get; }

    static abstract FeatureEff<RT, TOut> Handle(TIn input);
}

public interface Feature<RT, TIn> : Feature<RT, TIn, Unit>;
