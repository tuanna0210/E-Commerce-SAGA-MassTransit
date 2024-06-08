using BuildingBlocks.CQRS;

namespace Inventory.API.Inventories.Commands.PreserveInventory
{
    public record PreserveInventoryCommand(Guid OrderId) : ICommand<PreserveInventoryResult>;
    public record PreserveInventoryResult(bool IsSuccessfully);
    public class UpdateInventoryCommandHandler : ICommandHandler<PreserveInventoryCommand, PreserveInventoryResult>
    {
        public async Task<PreserveInventoryResult> Handle(PreserveInventoryCommand request, CancellationToken cancellationToken)
        {
            //TODO, update inventory logic here
            return new PreserveInventoryResult(true);
        }
    }
}
