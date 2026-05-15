using System;
using System.Collections.Generic;
using System.Text;

namespace VSlices.Monads
{
    public sealed partial class Flow<C, R, A>
    {
        
    }

    public partial class Flow<C, R>
    {
        public static Flow<C, R, C> Context { get; } = Asks((c, r) => c);
        
        public static Flow<C, R, R> Request { get; } = Asks((c, r) => r);
        
    }
}
