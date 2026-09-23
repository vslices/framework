namespace VSlices.Work;

public interface WorkProcess<P, ALG, REQ, RES>
    where ALG : Functor<ALG>
    where P : WorkProcess<P, ALG, REQ, RES>
{
    static abstract Free<ALG, RES> Get(REQ request);
}
