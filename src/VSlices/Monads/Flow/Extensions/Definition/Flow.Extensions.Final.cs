using System;
using System.Collections.Generic;
using System.Text;
using VSlices.Monads;

namespace VSlices.Monads
{
    public sealed partial class Flow<RT, REQ, RES>
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="X"></typeparam>
        /// <param name="mx"></param>
        /// <returns></returns>
        public Flow<RT, REQ, RES> Finally<X>(K<Flow<RT, REQ>, X> mx) =>
            Flow<RT, REQ>.Finally(this, mx);

    }
}

namespace VSlices
{
    public static partial class FlowExtensions
    {
        extension<C, R, A>(K<Flow<C, R>, A> ma)
        {
            /// <summary>
            ///
            /// </summary>
            /// <typeparam name="X"></typeparam>
            /// <param name="mx"></param>
            /// <returns></returns>
            public Flow<C, R, A> Finally<X>(K<Flow<C, R>, X> mx) =>
                ma.As().Finally(mx);
        }
    }
}
