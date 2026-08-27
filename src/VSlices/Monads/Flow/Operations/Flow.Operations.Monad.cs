using System;
using System.Collections.Generic;
using System.Text;
using VSlices.Monads;

namespace VSlices;

public static partial class FlowExtensions
{
    extension<RT, REQ, A, B>(K<Flow<RT, REQ>, A>)
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ma"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Flow<RT, REQ, B> operator >> (K<Flow<RT, REQ>, A> ma, Func<A, K<Flow<RT, REQ>, B>> f) =>
            ma.Bind(f);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ma"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Flow<RT, REQ, B> operator >>(K<Flow<RT, REQ>, A> ma, Func<A, K<Eff<RT>, B>> f) =>
            ma.Bind(f);
    }
}
