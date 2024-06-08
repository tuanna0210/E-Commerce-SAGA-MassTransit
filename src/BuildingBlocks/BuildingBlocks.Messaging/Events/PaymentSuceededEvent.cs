namespace BuildingBlocks.Messaging.Events;

public record PaymentSuceededEvent
{
    public Guid OrderId { get; init; }
}
