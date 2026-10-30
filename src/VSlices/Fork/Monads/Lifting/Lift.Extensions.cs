using System.Threading.Tasks;

namespace ForkExt;

public static class LiftExtensions
{
    public static IO<A> ToIO<A>(this Lift<EnvIO, A> f) =>
        IO.lift(f.Function);
    
    public static IO<A> ToIO<A>(this Lift<EnvIO, Task<A>> f) =>
        IO.liftAsync(f.Function);    
        
    public static IO<A> ToIO<A>(this Lift<Task<A>> f) =>
        IO.liftAsync(f.Function);
}
