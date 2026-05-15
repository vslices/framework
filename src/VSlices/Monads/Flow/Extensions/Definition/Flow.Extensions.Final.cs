using System;
using System.Collections.Generic;
using System.Text;
using VSlices.Monads;

namespace VSlices.Monads
{
    public sealed partial class Flow<C, R, A>
    {
        public Flow<C, R, A> Finally<X>(K<Flow<C, R>, X> mx) =>
            Flow<C, R>.Finally(this, mx);
    }
}

namespace VSlices
{
    public static partial class FlowExtensions
    {
        extension<C, R, A>(K<Flow<C, R>, A> ma)
        {
            public Flow<C, R, A> Finally<X>(K<Flow<C, R>, X> mx) =>
                ma.As().Finally(mx);
        }
    }
}
