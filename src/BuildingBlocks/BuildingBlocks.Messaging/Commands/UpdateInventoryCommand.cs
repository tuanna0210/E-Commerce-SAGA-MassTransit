namespace BuildingBlocks.Messaging.Messages;

//[EntityName("preserve-products-command")]
public class UpdateInventoryCommand
{
    public Guid OrderId { get; set; }
}
