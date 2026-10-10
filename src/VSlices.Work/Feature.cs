namespace VSlices.Work;

public interface Feature<ALG, REQ, RES>
    where ALG : Functor<ALG>
{
    static abstract Free<ALG, RES> Describe(REQ request);
}
