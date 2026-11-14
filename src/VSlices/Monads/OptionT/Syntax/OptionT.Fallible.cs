// Resharper disable CheckNamespace
namespace LanguageExt;

/// <summary>
/// Interpret absence in an OptionT as a failure of its underlying monad.
/// A present value is preserved and failures from the outer monad propagate.
/// </summary>
public static class OptionTFallibleExtensions
{
    extension<M, A>(OptionT<M, A> optional)
        where M : Monad<M>, Fallible<M>
    {
        /// <summary>
        /// Keep Some(a) as M.Pure(a), or raise the lazily supplied error for None.
        /// </summary>
        public K<M, A> IfNone(Func<Error> fe) =>
            optional.Run().Bind(option =>
                option.Match(
                    Some: M.Pure,
                    None: () => M.Fail<A>(fe())));
    }
}
