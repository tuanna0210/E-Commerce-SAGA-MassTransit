namespace BuildingBlocks.Messaging.Events;

public record OrderCreatedEvent
{
    public Guid OrderId { get; init; }
    public Guid UserId { get; init; }
}
