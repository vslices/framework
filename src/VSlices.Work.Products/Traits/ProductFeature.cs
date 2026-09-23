using LanguageExt.Traits;
using VSlices.Products;

namespace VSlices.Work;

public interface ProductFeature<F, ALG, REQ, RES> :
    Feature<F, ALG, REQ, RES>
    where ALG : Functor<ALG>
    where F : ProductFeature<F, ALG, REQ, RES>
{
    static abstract ProductRole ExecutableBy { get; }
}
