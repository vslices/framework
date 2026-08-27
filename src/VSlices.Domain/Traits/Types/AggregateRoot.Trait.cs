namespace VSlices.Domain.Traits;

public interface AggregateRoot<SELF> : Entity<SELF>
    where SELF : AggregateRoot<SELF>;

public interface AggregateRoot<SELF, ID> : AggregateRoot<SELF>, Entity<SELF, ID>
    where SELF : AggregateRoot<SELF, ID>
    where ID : Identifier<ID>;

public interface AggregateRoot<SELF, ID, REPR> : AggregateRoot<SELF, ID>, Entity<SELF, ID, REPR>
    where SELF : AggregateRoot<SELF, ID, REPR>
    where ID : Identifier<ID>;

