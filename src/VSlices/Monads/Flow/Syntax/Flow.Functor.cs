// Resharper disable CheckNamespace
using VSlices.Monads;

namespace VSlices;

public static partial class FlowFluentAPISyntax
{
    extension<RT, RQ, A>(K<Flow<RT, RQ>, A> ma)
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="B"></typeparam>
        /// <param name="fb"></param>
        /// <returns></returns>
        public Flow<RT, RQ, B> Map<B>(Func<A, B> fb) =>
            Flow<RT, RQ>.Map(fb, ma);

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="B"></typeparam>
        /// <param name="b"></param>
        /// <returns></returns>
        public Flow<RT, RQ, B> ConstMap<B>(B b) =>
            Flow<RT, RQ>.ConstMap(b, ma);

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="B"></typeparam>
        /// <param name="pb"></param>
        /// <returns></returns>
        public Flow<RT, RQ, B> ConstMap<B>(Pure<B> pb) =>
            Flow<RT, RQ>.ConstMap(pb.Value, ma);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public Flow<RT, RQ, Unit> Ignore() =>
            ma.As().ConstMap(unit);
    }
}

public static partial class FlowOperatorSyntax
{
    extension<RT, RQ, A, B>(K<Flow<RT, RQ>, A> ma)
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="m"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Flow<RT, RQ, B> operator *(
            K<Flow<RT, RQ>, A> m,
            Func<A, B> f) =>
            +Flow<RT, RQ>.Map(f, m);

        /// <summary>
        ///
        /// </summary>
        /// <param name="f"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public static Flow<RT, RQ, B> operator *(
            Func<A, B> f,
            K<Flow<RT, RQ>, A> m) =>
            +Flow<RT, RQ>.Map(f, m);

        /// <summary>
        ///
        /// </summary>
        /// <param name="p"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public static Flow<RT, RQ, B> operator *(
            Pure<B> p,
            K<Flow<RT, RQ>, A> m) =>
            +Flow<RT, RQ>.ConstMap(p.Value, m);

        /// <summary>
        ///
        /// </summary>
        /// <param name="m"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Flow<RT, RQ, B> operator *(
            K<Flow<RT, RQ>, A> m,
            Pure<B> p) =>
            +Flow<RT, RQ>.ConstMap(p.Value, m);
    }
}

public static partial class FlowLinqSyntax
{
    extension<RT, RQ, A>(K<Flow<RT, RQ>, A> ma)
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="B"></typeparam>
        /// <param name="fb"></param>
        /// <returns></returns>
        public Flow<RT, RQ, B> Select<B>(Func<A, B> fb) =>
            ma.Map(fb);
    }
}
