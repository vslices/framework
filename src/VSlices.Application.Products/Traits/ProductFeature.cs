using VSlices.Products;

namespace VSlices.Application;

public interface ProductFeature<F, RT, REQ, RES> : Feature<F, RT, REQ, RES>
    where F : ProductFeature<F, RT, REQ, RES>
{
    static abstract ProductRole ExecutableBy { get; }
}

public interface ProductFeature<F, RT, REQ> : ProductFeature<F, RT, REQ, Unit>
    where F : ProductFeature<F, RT, REQ>;
