namespace BuildingBlocks.Messaging.Events;
//[EntityName("preserve-products-failed-event")]
public class InventoryPreserveFailedEvent
{
    public Guid OrderId { get; init; }
}
