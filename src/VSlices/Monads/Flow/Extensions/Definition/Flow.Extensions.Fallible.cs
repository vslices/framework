using VSlices.Monads;

namespace VSlices.Monads
{
    public sealed partial class Flow<RT, REQ, RES>
    {
        // Catch(Func<Error, Error>);
        // Catch(Func<Error, Fail<Error>);

        // Catch(Func<Error, IO<RES>>);
        // Catch(Func<Error, Eff<RES>>);
        // Catch(Func<Error, Eff<RT, RES>>);
        // Catch(Func<Error, Fin<RES>>);
        // Catch(Func<Error, FinT<IO, RES>>);
        // Catch(Func<Error, FinT<Eff, RES>>);
        // Catch(Func<Error, FinT<Eff<RT>, RES>>);
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
