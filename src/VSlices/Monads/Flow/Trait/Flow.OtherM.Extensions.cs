using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using VSlices.Monads;

namespace LanguageExt;

public static partial class FlowExtensions
{
    extension<RES>(K<Eff, RES> ma)
    {
        /// <summary>
        /// Converts a monadic effect of type <see cref="K{Eff, RES}"/> into a flow representation.
        /// </summary>
        /// <typeparam name="RT">The type of the runtime environment used in the flow.</typeparam>
        /// <typeparam name="REQ">The type of the request input for the flow.</typeparam>
        /// <returns>
        /// A <see cref="Flow{RT, REQ, RES}"/> instance that encapsulates the provided monadic effect.
        /// </returns>
        public Flow<RT, REQ, RES> ToFlow<RT, REQ>() =>
            Flow<RT, REQ>.Lift(ma);
    }

    extension<RT, RES>(K<Eff<RT>, RES> ma)
    {
        /// <summary>
        /// Converts the specified monadic effect into a <see cref="Flow{RT, REQ, RES}"/> instance.
        /// </summary>
        /// <typeparam name="REQ">The type of the request input for the resulting flow.</typeparam>
        /// <returns>
        /// A <see cref="Flow{RT, REQ, RES}"/> instance that encapsulates the given monadic effect.
        /// </returns>
        public Flow<RT, REQ, RES> ToFlow<REQ>() =>
            Flow<RT, REQ>.Lift(ma);
    }

    extension<RES>(K<Fin, RES> ma)
    {
        /// <summary>
        /// Converts a monadic computation of type <see cref="Fin{RES}"/> into a flow computation.
        /// </summary>
        /// <typeparam name="RT">The type of the runtime environment used in the flow.</typeparam>
        /// <typeparam name="REQ">The type of the request input for the flow.</typeparam>
        /// <returns>
        /// A <see cref="Flow{RT, REQ, RES}"/> instance that represents the lifted monadic computation.
        /// </returns>
        public Flow<RT, REQ, RES> ToFlow<RT, REQ>() =>
            Flow<RT, REQ>.Lift(ma);
    }
    
    extension<RES>(K<FinT<IO>, RES> ma)
    {
        /// <summary>
        /// Converts a monadic value of type <see cref="FinT{M, A}"/> into a <see cref="Flow{RT, REQ, RES}"/>.
        /// </summary>
        /// <typeparam name="RT">The type of the runtime environment used in the flow.</typeparam>
        /// <typeparam name="REQ">The type of the request input for the flow.</typeparam>
        /// <returns>
        /// A <see cref="Flow{RT, REQ, RES}"/> instance that encapsulates the provided monadic value.
        /// </returns>
        public Flow<RT, REQ, RES> ToFlow<RT, REQ>() =>
            Flow<RT, REQ>.Lift(ma);
    }

    extension<RES>(K<FinT<Eff>, RES> ma)
    {
        /// <summary>
        /// Converts a monadic value of type <see cref="FinT{M, A}"/> into a <see cref="Flow{RT, REQ, RES}"/>.
        /// </summary>
        /// <typeparam name="RT">The type of the runtime environment used in the flow.</typeparam>
        /// <typeparam name="REQ">The type of the request input for the flow.</typeparam>
        /// <returns>
        /// A <see cref="Flow{RT, REQ, RES}"/> instance that encapsulates the provided monadic value.
        /// </returns>
        public Flow<RT, REQ, RES> ToFlow<RT, REQ>() =>
            Flow<RT, REQ>.Lift(ma);
    }

    extension<RT, RES>(K<FinT<Eff<RT>>, RES> ma)
    {
        /// <summary>
        /// Converts the specified monadic value into a <see cref="Flow{RT, REQ, RES}"/> instance.
        /// </summary>
        /// <typeparam name="REQ">The type of the request input for the resulting flow.</typeparam>
        /// <returns>
        /// A <see cref="Flow{RT, REQ, RES}"/> instance that encapsulates the provided monadic value.
        /// </returns>
        public Flow<RT, REQ, RES> ToFlow<REQ>() =>
            Flow<RT, REQ>.Lift(ma);
    }
}
