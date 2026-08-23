//using VSlices.Monads;

//namespace VSlices.Domain.Traits;

///// <summary>
/////
///// </summary>
///// <typeparam name="SELF">
/////
///// </typeparam>
///// <typeparam name="M">
/////
///// </typeparam>
///// <typeparam name="TYPE">
/////
///// </typeparam>
///// <typeparam name="IN">
/////
///// </typeparam>
///// <remarks>
/////
///// </remarks>
//public interface TransformM<SELF, M, TYPE, IN> : DomainType<SELF>
//    where SELF : TransformM<SELF, M, TYPE, IN>
//    where M : Monad<M>
//    where TYPE : DomainType<TYPE>
//{
//    /// <summary>
//    ///
//    /// </summary>
//    public static abstract ReqT<M, IN, TYPE> Apply { get; }

//    /// <summary>
//    ///
//    /// </summary>
//    /// <param name="repr">
//    ///
//    /// </param>
//    /// <returns>
//    ///
//    /// </returns>
//    public static virtual FinT<M, TYPE> Create(IN repr) =>
//        SELF.Apply.Onto(repr);

//    /// <summary>
//    ///
//    /// </summary>
//    /// <param name="repr">
//    ///
//    /// </param>
//    /// <returns>
//    ///
//    /// </returns>
//    public static virtual K<M, TYPE> New(IN repr) =>
//        SELF.Create(repr)
//            .Run()
//            .Map(f => f.ThrowIfFail());

//}

///// <inheritdoc/>
//public interface TransformM<SELF, M, IN> : TransformM<SELF, M, SELF, IN>
//    where SELF : TransformM<SELF, M, IN>
//    where M : Monad<M>;
