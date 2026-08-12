// Resharper disable CheckNamespace
using VSlices.Monads;

namespace VSlices;

public static partial class FlowExtensions
{
    extension<C, R, I, O>(K<Flow<C, R>, I>)
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="m"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Flow<C, R, O> operator *(
            K<Flow<C, R>, I> m,
            Func<I, O> f) =>
            +Flow<C, R>.Map(f, m);

        /// <summary>
        ///
        /// </summary>
        /// <param name="f"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public static Flow<C, R, O> operator *(
            Func<I, O> f,
            K<Flow<C, R>, I> m) =>
            +Flow<C, R>.Map(f, m);

        /// <summary>
        ///
        /// </summary>
        /// <param name="p"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public static Flow<C, R, O> operator *(
            Pure<O> p,
            K<Flow<C, R>, I> m) =>
            +Flow<C, R>.ConstMap(p.Value, m);

        /// <summary>
        ///
        /// </summary>
        /// <param name="m"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Flow<C, R, O> operator *(
            K<Flow<C, R>, I> m,
            Pure<O> p) =>
            +Flow<C, R>.ConstMap(p.Value, m);
    }

    extension<C, R, I1, I2, O>(K<Flow<C, R>, I1>)
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="m"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Flow<C, R, Func<I2, O>> operator *(
            K<Flow<C, R>, I1> m,
            Func<I1, I2, O> f) =>
            m * curry(f);

        /// <summary>
        ///
        /// </summary>
        /// <param name="f"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public static Flow<C, R, Func<I2, O>> operator *(
            Func<I1, I2, O> f,
            K<Flow<C, R>, I1> m) =>
            m * curry(f);
    }

    extension<C, R, I1, I2, I3, O>(K<Flow<C, R>, I1>)
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="m"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Flow<C, R, Func<I2, Func<I3, O>>> operator *(
            K<Flow<C, R>, I1> m,
            Func<I1, I2, I3, O> f) =>
            m * curry(f);

        /// <summary>
        ///
        /// </summary>
        /// <param name="f"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public static Flow<C, R, Func<I2, Func<I3, O>>> operator *(
            Func<I1, I2, I3, O> f,
            K<Flow<C, R>, I1> m) =>
            m * curry(f);
    }

    extension<C, R, I1, I2, I3, I4, O>(K<Flow<C, R>, I1>)
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="m"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Flow<C, R, Func<I2, Func<I3, Func<I4, O>>>> operator *(
            K<Flow<C, R>, I1> m,
            Func<I1, I2, I3, I4, O> f) =>
            m * curry(f);

        /// <summary>
        ///
        /// </summary>
        /// <param name="f"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public static Flow<C, R, Func<I2, Func<I3, Func<I4, O>>>> operator *(
            Func<I1, I2, I3, I4, O> f,
            K<Flow<C, R>, I1> m) =>
            m * curry(f);
    }

    extension<C, R, I1, I2, I3, I4, I5, O>(K<Flow<C, R>, I1>)
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="f"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public static Flow<C, R, Func<I2, Func<I3, Func<I4, Func<I5, O>>>>> operator *(
            Func<I1, I2, I3, I4, I5, O> f,
            K<Flow<C, R>, I1> m) =>
            m * curry(f);

        /// <summary>
        ///
        /// </summary>
        /// <param name="m"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Flow<C, R, Func<I2, Func<I3, Func<I4, Func<I5, O>>>>> operator *(
            K<Flow<C, R>, I1> m,
            Func<I1, I2, I3, I4, I5, O> f) =>
            m * curry(f);
    }

    extension<C, R, I1, I2, I3, I4, I5, I6, O>(K<Flow<C, R>, I1>)
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="m"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Flow<C, R, Func<I2, Func<I3, Func<I4, Func<I5, Func<I6, O>>>>>> operator *(
            K<Flow<C, R>, I1> m,
            Func<I1, I2, I3, I4, I5, I6, O> f) =>
            m * curry(f);

        /// <summary>
        ///
        /// </summary>
        /// <param name="f"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public static Flow<C, R, Func<I2, Func<I3, Func<I4, Func<I5, Func<I6, O>>>>>> operator *(
            Func<I1, I2, I3, I4, I5, I6, O> f,
            K<Flow<C, R>, I1> m) =>
            m * curry(f);
    }

    extension<C, R, I1, I2, I3, I4, I5, I6, I7, O>(K<Flow<C, R>, I1>)
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="m"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Flow<C, R, Func<I2, Func<I3, Func<I4, Func<I5, Func<I6, Func<I7, O>>>>>>> operator *(
            K<Flow<C, R>, I1> m,
            Func<I1, I2, I3, I4, I5, I6, I7, O> f) =>
            m * curry(f);

        /// <summary>
        ///
        /// </summary>
        /// <param name="f"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public static Flow<C, R, Func<I2, Func<I3, Func<I4, Func<I5, Func<I6, Func<I7, O>>>>>>> operator *(
            Func<I1, I2, I3, I4, I5, I6, I7, O> f,
            K<Flow<C, R>, I1> m) =>
            m * curry(f);
    }
}
