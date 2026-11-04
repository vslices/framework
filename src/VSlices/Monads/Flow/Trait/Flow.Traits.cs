using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ForkExt;
using ForkExt.Common;
using ForkExt.Traits;
using ForkExt.Effects;
using ForkExt.Traits.Domain;
using static ForkExt.Prelude;
using VSlices;
using VSlices.Monads;
using static VSlices.VSlicesPrelude;
using System;
using System.Collections.Generic;
using System.Text;

namespace VSlices.Monads;

/// <summary>
/// Represents a monadic liftFlow that encapsulates computations with a specific runtime and request context.
/// </summary>
/// <typeparam name="ALG">The type of the executable algebra used in the liftFlow.</typeparam>
/// <typeparam name="RQ">The type of the request context used in the liftFlow.</typeparam>
public partial class Flow<ALG, RQ> :
    MonadUnliftIO<Flow<ALG, RQ>>,
    Fallible<Error, Flow<ALG, RQ>>,
    Alternative<Flow<ALG, RQ>>,
    MonoidK<Flow<ALG, RQ>>,
    Final<Flow<ALG, RQ>>,
    Readable<Flow<ALG, RQ>, (ALG, RQ)>
{
    private Flow() {}
}
