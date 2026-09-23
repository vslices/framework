namespace VSlices.Work;

public interface Feature<F, ALG, REQ, RES>
    where ALG : Functor<ALG>
    where F : Feature<F, ALG, REQ, RES>
{
    static abstract Free<ALG, RES> Get(REQ request);
}
