namespace VSlices.Domain.Traits;

public interface Entity<SELF> : DomainType<SELF>
    where SELF : Entity<SELF>;

public interface Entity<SELF, ID> : Entity<SELF>
    where SELF : Entity<SELF, ID>
    where ID : Identifier<ID>
{
    ID Id { get; }
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="SELF"></typeparam>
/// <typeparam name="REPR"></typeparam>
/// <typeparam name="ID"></typeparam>
public interface Entity<SELF, ID, REPR> : Entity<SELF, ID>
    where SELF : Entity<SELF, ID, REPR>
    where ID : Identifier<ID>
{
    /// <summary>
    /// 
    /// </summary>
    ID Id { get; }
}
