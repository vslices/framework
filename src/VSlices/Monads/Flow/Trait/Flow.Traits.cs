using System;
using System.Collections.Generic;
using System.Text;

namespace VSlices.Monads;

/// <summary>
/// Represents a monadic liftFlow that encapsulates computations with a specific runtime and request context.
/// </summary>
/// <typeparam name="RT">The type of the runtime context used in the liftFlow.</typeparam>
/// <typeparam name="RQ">The type of the request context used in the liftFlow.</typeparam>
public partial class Flow<RT, RQ> :
    MonadUnliftIO<Flow<RT, RQ>>,
    Fallible<Error, Flow<RT, RQ>>,
    Alternative<Flow<RT, RQ>>,
    MonoidK<Flow<RT, RQ>>,
    Final<Flow<RT, RQ>>,
    Readable<Flow<RT, RQ>, (RT, RQ)>
{
    private Flow() {}
}
