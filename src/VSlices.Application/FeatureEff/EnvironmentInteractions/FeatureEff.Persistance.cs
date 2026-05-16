//using System.Security.Cryptography;
//using VSlices.Domain.Envs;
//using VSlices.Monads;

//namespace VSlices;

//public static partial class VSlicesAppPrelude
//{
//    public static Flow<RT, REQ, RES> transactionScope<RT, REQ, RES>(
//        K<Flow<RT, REQ>, RES> ma)
//        where RT : HasPersistence<RT> => 
//        from _0 in Flow<RT, REQ>.Lift(PersistenceEnv<RT>.start())
//        from a in ma.As().Finally()As().FullBind(
//            Succ: Flow<RT, RES> (RES v) => PersistenceEnv<RT>.commit().Map(_ => v),
//            Fail: Flow<RT, RES> (fe) => PersistenceEnv<RT>.rollback()
//                                                              .Map(_ => Fail(fe).ToEither<RES>()),
//            Except: Flow<RT, RES> (ex) => PersistenceEnv<RT>.rollback()
//                                                                .Bind(_ => Fail(ex).ToEff<A>()))

//        select a;

//    public static Flow<RT, REQ, RES> InTransaction<RT, REQ, RES>(this K<Flow<RT, REQ>, RES> ma)
//        where RT : HasPersistence<RT> =>
//        transactionScope(ma);

//}
