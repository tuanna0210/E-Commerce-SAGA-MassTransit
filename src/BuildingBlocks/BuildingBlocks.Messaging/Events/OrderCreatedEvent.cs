namespace BuildingBlocks.Messaging.Events;

//[EntityName("order-created-event")]
public record OrderCreatedEvent
{
    public Guid OrderId { get; init; }
    public Guid CustomerId { get; init; }
}
