using System;
using System.Collections.Generic;
using System.Text;

namespace VSlices.Monads
{
    public sealed partial class Flow<RT, REQ, RES>
    {

    }

    public partial class Flow<RT, REQ>
    {
        /// <summary>
        ///
        /// </summary>
        public static Flow<RT, REQ, RT> Context { get; } = Asks((c, r) => c);

        /// <summary>
        ///
        /// </summary>
        public static Flow<RT, REQ, REQ> Request { get; } = Asks((c, r) => r);

    }
}
