using System;
using ForkExt.Traits;

namespace ForkExt;

public static class ReadableExtensions
{
    public static K<M, A> Local<M, Env, A>(this K<M, A> ma, Func<Env, Env> f)
        where M : Readable<M, Env> =>
        M.Local(f, ma);
}
