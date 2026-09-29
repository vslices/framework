// Resharper disable CheckNamespace
using VSlices.Monads;

namespace VSlices;

public static partial class FlowFluentAPISyntax
{
    extension<ALG, RQ, A>(K<Flow<ALG, RQ>, A> ma)
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="X"></typeparam>
        /// <param name="mx"></param>
        /// <returns></returns>
        public Flow<ALG, RQ, A> Finally<X>(K<Flow<ALG, RQ>, X> mx) =>
            Flow<ALG, RQ>.Finally(ma, mx);
    }
}

public static partial class FlowLinqSyntax
{
    extension<ALG, RQ, A>(K<Flow<ALG, RQ>, A> ma)
    {
        
    }
}

public static partial class FlowOperatorSyntax
{
    extension<ALG, RQ, A, X>(K<Flow<ALG, RQ>, A>)
    {
        /// <summary>
        /// Run a `finally` operation after the main operation regardless of whether it succeeds or not.
        /// </summary>
        /// <param name="lhs">Primary operation</param>
        /// <param name="rhs">Finally operation</param>
        /// <returns>Result of primary operation</returns>
        public static Flow<ALG, RQ, A> operator |(
            K<Flow<ALG, RQ>, A> lhs, 
            Finally<Flow<ALG, RQ>, X> rhs) =>
            lhs.Finally(rhs.Operation);
    }
}
