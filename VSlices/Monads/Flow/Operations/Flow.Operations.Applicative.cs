// Resharper disable CheckNamespace

using VSlices.Monads;

namespace VSlices;

public static partial class FlowExtensions
{

    extension<C, R, I, O>(K<Flow<C, R>, I>)
    {
        public static Flow<C, R, O> operator >>> (
            K<Flow<C, R>, I> ma,
            K<Flow<C, R>, O> mb) =>
            +Applicative.action(ma, mb);

        public static Flow<C, R, O> operator * (
            K<Flow<C, R>, Func<I, O>> mf,
            K<Flow<C, R>, I> ma) =>
            +Applicative.apply(mf, ma);

        public static Flow<C, R, O> operator * (
            K<Flow<C, R>, I> ma,
            K<Flow<C, R>, Func<I, O>> mf) =>
            +Applicative.apply(mf, ma);
    }

    extension<C, R, I1, I2, O>(K<Flow<C, R>, I1>)
    {
        public static K<Flow<C, R>, Func<I2, O>> operator *(
            K<Flow<C, R>, Func<I1, I2, O>> mf,
            K<Flow<C, R>, I1> ma) =>
            curry * mf * ma;
        public static K<Flow<C, R>, Func<I2, O>> operator *(
            K<Flow<C, R>, I1> ma,
            K<Flow<C, R>, Func<I1, I2, O>> mf) =>
            curry * mf * ma;

    }

    extension<C, R, I1, I2, I3, O>(K<Flow<C, R>, I1>)
    {
        public static K<Flow<C, R>, Func<I2, Func<I3, O>>> operator *(
            K<Flow<C, R>, Func<I1, I2, I3, O>> mf,
            K<Flow<C, R>, I1> ma) =>
            curry * mf * ma;
        public static K<Flow<C, R>, Func<I2, Func<I3, O>>> operator *(
            K<Flow<C, R>, I1> ma,
            K<Flow<C, R>, Func<I1, I2, I3, O>> mf) =>
            curry * mf * ma;

    }

    extension<C, R, I1, I2, I3, I4, O>(K<Flow<C, R>, I1>)
    {
        public static K<Flow<C, R>, Func<I2, Func<I3, Func<I4, O>>>> operator *(
            K<Flow<C, R>, Func<I1, I2, I3, I4, O>> mf,
            K<Flow<C, R>, I1> ma) =>
            curry * mf * ma;
        public static K<Flow<C, R>, Func<I2, Func<I3, Func<I4, O>>>> operator *(
            K<Flow<C, R>, I1> ma,
            K<Flow<C, R>, Func<I1, I2, I3, I4, O>> mf) =>
            curry * mf * ma;

    }

    extension<C, R, I1, I2, I3, I4, I5, O>(K<Flow<C, R>, I1>)
    {
        public static K<Flow<C, R>, Func<I2, Func<I3, Func<I4, Func<I5, O>>>>> operator *(
            K<Flow<C, R>, Func<I1, I2, I3, I4, I5, O>> mf,
            K<Flow<C, R>, I1> ma) =>
            curry * mf * ma;
        public static K<Flow<C, R>, Func<I2, Func<I3, Func<I4, Func<I5, O>>>>> operator *(
            K<Flow<C, R>, I1> ma,
            K<Flow<C, R>, Func<I1, I2, I3, I4, I5, O>> mf) =>
            curry * mf * ma;

    }

    extension<C, R, I1, I2, I3, I4, I5, I6, O>(K<Flow<C, R>, I1>)
    {
        public static K<Flow<C, R>, Func<I2, Func<I3, Func<I4, Func<I5, Func<I6, O>>>>>> operator *(
            K<Flow<C, R>, Func<I1, I2, I3, I4, I5, I6, O>> mf,
            K<Flow<C, R>, I1> ma) =>
            curry * mf * ma;
        public static K<Flow<C, R>, Func<I2, Func<I3, Func<I4, Func<I5, Func<I6, O>>>>>> operator *(
            K<Flow<C, R>, I1> ma,
            K<Flow<C, R>, Func<I1, I2, I3, I4, I5, I6, O>> mf) =>
            curry * mf * ma;

    }

    extension<C, R, I1, I2, I3, I4, I5, I6, I7, O>(K<Flow<C, R>, I1>)
    {
        public static K<Flow<C, R>, Func<I2, Func<I3, Func<I4, Func<I5, Func<I6, Func<I7, O>>>>>>> operator *(
            K<Flow<C, R>, Func<I1, I2, I3, I4, I5, I6, I7, O>> mf,
            K<Flow<C, R>, I1> ma) =>
            curry * mf * ma;
        public static K<Flow<C, R>, Func<I2, Func<I3, Func<I4, Func<I5, Func<I6, Func<I7, O>>>>>>> operator *(
            K<Flow<C, R>, I1> ma,
            K<Flow<C, R>, Func<I1, I2, I3, I4, I5, I6, I7, O>> mf) =>
            curry * mf * ma;

    }
}
