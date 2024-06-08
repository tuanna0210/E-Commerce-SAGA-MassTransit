namespace BuildingBlocks.Messaging.Events
{
    public record PaymentFailedEvent
    {
        public Guid OrderId { get; init; }
    }
}
