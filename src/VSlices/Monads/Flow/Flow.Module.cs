using System;
using System.Collections.Generic;
using System.Text;
using LanguageExt.Core;
using VSlices.Arrows;

namespace VSlices.Monads;

/// <summary>
/// Provides a collection of static methods and properties for working with the <see cref="Flow{RT, REQ, RES}"/> monad
/// </summary>
public static partial class Flow
{
    public static Flow<RT, REQ, RT> runtime<RT, REQ>() =>
        Flow<RT>.runtime<REQ>();

    public static Flow<RT, REQ, REQ> request<RT, REQ>() =>
        Flow<RT>.request<REQ>();
}

/// <summary>
/// Provides a collection of static methods and properties for working with the <see cref="Flow{RT, REQ, RES}"/> monad
/// </summary>
/// <typeparam name="RT">
/// The type of the runtime environment used in the flow
/// </typeparam>
public static partial class Flow<RT>
{
    public static Flow<RT, REQ, RT> runtime<REQ>() =>
        Flow<RT, REQ>.runtime;

    public static Flow<RT, REQ, REQ> request<REQ>() =>
        Flow<RT, REQ>.request;
}

public partial class Flow<RT, REQ>
{ 
    public static readonly Flow<RT, REQ, RT> runtime =
        liftFlow((RT rt, REQ _) => rt);

    public static readonly Flow<RT, REQ, REQ> request =
        liftFlow((RT _, REQ r) => r);
}
