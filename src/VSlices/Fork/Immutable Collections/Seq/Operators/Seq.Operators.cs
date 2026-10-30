using ForkExt.Traits;

namespace ForkExt;

public static partial class SeqExtensions
{
    extension<A>(K<Seq, A> _)
    {
        /// <summary>
        /// Downcast operator
        /// </summary>
        public static Seq<A> operator +(K<Seq, A> ma) =>
            (Seq<A>)ma;
        
        public static Seq<A> operator >> (K<Seq, A> ma, Lower lower) =>
            (Seq<A>)ma;
    }
}
