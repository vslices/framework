// Resharper disable CheckNamespace
using VSlices.Monads;

namespace VSlices;

public static partial class FlowFluentAPISyntax
{
    extension<ALG, RQ, A>(K<Flow<ALG, RQ>, A> ma)
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="B"></typeparam>
        /// <param name="fb"></param>
        /// <returns></returns>
        public Flow<ALG, RQ, B> Map<B>(Func<A, B> fb) =>
            Flow<ALG, RQ>.Map(fb, ma);

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="B"></typeparam>
        /// <param name="b"></param>
        /// <returns></returns>
        public Flow<ALG, RQ, B> ConstMap<B>(B b) =>
            Flow<ALG, RQ>.ConstMap(b, ma);

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="B"></typeparam>
        /// <param name="pb"></param>
        /// <returns></returns>
        public Flow<ALG, RQ, B> ConstMap<B>(Pure<B> pb) =>
            Flow<ALG, RQ>.ConstMap(pb.Value, ma);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public Flow<ALG, RQ, Unit> Ignore() =>
            ma.As().ConstMap(unit);
    }
}

public static partial class FlowOperatorSyntax
{
    extension<ALG, RQ, A, B>(K<Flow<ALG, RQ>, A> ma)
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="m"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Flow<ALG, RQ, B> operator *(
            K<Flow<ALG, RQ>, A> m,
            Func<A, B> f) =>
            +Flow<ALG, RQ>.Map(f, m);

        /// <summary>
        ///
        /// </summary>
        /// <param name="f"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public static Flow<ALG, RQ, B> operator *(
            Func<A, B> f,
            K<Flow<ALG, RQ>, A> m) =>
            +Flow<ALG, RQ>.Map(f, m);

        /// <summary>
        ///
        /// </summary>
        /// <param name="p"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public static Flow<ALG, RQ, B> operator *(
            Pure<B> p,
            K<Flow<ALG, RQ>, A> m) =>
            +Flow<ALG, RQ>.ConstMap(p.Value, m);

        /// <summary>
        ///
        /// </summary>
        /// <param name="m"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Flow<ALG, RQ, B> operator *(
            K<Flow<ALG, RQ>, A> m,
            Pure<B> p) =>
            +Flow<ALG, RQ>.ConstMap(p.Value, m);
    }
}

public static partial class FlowLinqSyntax
{
    extension<ALG, RQ, A>(K<Flow<ALG, RQ>, A> ma)
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="B"></typeparam>
        /// <param name="fb"></param>
        /// <returns></returns>
        public Flow<ALG, RQ, B> Select<B>(Func<A, B> fb) =>
            ma.Map(fb);
    }
}
