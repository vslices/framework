using VSlices.Monads;

namespace VSlices.Monads
{
    public sealed partial class Flow<C, R, A>
    {
        // Catch(Func<Error, Error>);
        // Catch(Func<Error, Fail<Error>);

        // Catch(Func<Error, IO<A>>);
        // Catch(Func<Error, Eff<A>>);
        // Catch(Func<Error, Eff<C, A>>);
        // Catch(Func<Error, Fin<A>>);
        // Catch(Func<Error, FinT<IO, A>>);
        // Catch(Func<Error, FinT<Eff, A>>);
        // Catch(Func<Error, FinT<Eff<C>, A>>);
    }
}

namespace VSlices
{
    public static partial class FlowExtensions
    {
        extension<C, R, A>(K<Flow<C, R>, A> ma)
        {

        }
    }
}