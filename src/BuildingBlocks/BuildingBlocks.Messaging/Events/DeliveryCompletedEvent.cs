namespace BuildingBlocks.Messaging.Events
{
    public record DeliveryCompletedEvent
    {
        public Guid OrderId { get; init; }
        public string DeliveredBy { get; set; } = null!;
    }
}
