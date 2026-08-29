// Resharper disable CheckNamespace
using VSlices.Monads;

namespace VSlices;

public static partial class FlowFluentAPISyntax
{
    extension<RT, RQ, A>(K<Flow<RT, RQ>, A> ma)
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="X"></typeparam>
        /// <param name="mx"></param>
        /// <returns></returns>
        public Flow<RT, RQ, A> Finally<X>(K<Flow<RT, RQ>, X> mx) =>
            Flow<RT, RQ>.Finally(ma, mx);
    }
}

public static partial class FlowLinqSyntax
{
    extension<RT, RQ, A>(K<Flow<RT, RQ>, A> ma)
    {
        
    }
}

public static partial class FlowOperatorSyntax
{
    extension<RT, RQ, A, X>(K<Flow<RT, RQ>, A>)
    {
        /// <summary>
        /// Run a `finally` operation after the main operation regardless of whether it succeeds or not.
        /// </summary>
        /// <param name="lhs">Primary operation</param>
        /// <param name="rhs">Finally operation</param>
        /// <returns>Result of primary operation</returns>
        public static Flow<RT, RQ, A> operator |(
            K<Flow<RT, RQ>, A> lhs, 
            Finally<Flow<RT, RQ>, X> rhs) =>
            lhs.Finally(rhs.Operation);
    }
}
