using LanguageExt;

namespace VSlices.Work;

/// <summary>
/// A Feature owns a nominal Request and Response and returns an effectful
/// computation over its module's executable algebra.
/// </summary>
/// <remarks>
/// Requests are constructed by callers. Responses are constructed by their
/// owning Feature after execution has produced the modeled result.
/// </remarks>
public interface Feature<F, ALG, REQ, RES>
    where F : Feature<F, ALG, REQ, RES>
{
    static abstract Eff<ALG, RES> Get(REQ request);
}
