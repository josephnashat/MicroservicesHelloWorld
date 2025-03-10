namespace Ordering.Domain.Abastractions;
//public interface IAggregate<T> : IAggregate, IEntity<T>
//{

//}
public interface IAggregate//: IEntity
{
    public IReadOnlyList<IDomainEvent> DomainEvents { get; }
    public void AddDomainEvent(IDomainEvent domainEvent);
    public IDomainEvent[] ClearDomainEvents();

}
