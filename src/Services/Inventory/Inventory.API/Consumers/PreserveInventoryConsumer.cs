using BuildingBlocks.Messaging.Events;
using BuildingBlocks.Messaging.Messages;
using Inventory.API.Contracts.Configurations;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Options;

namespace Inventory.API.Consumers
{
    public class PreserveInventoryConsumer(ISender sender) : IConsumer<UpdateInventoryCommand>
    {
        public async Task Consume(ConsumeContext<UpdateInventoryCommand> context)
        {
            Guid OrderId = context.Message.OrderId;
            Console.WriteLine("UpdateInventoryConsumer For Order {0}", OrderId);
            var result = await sender.Send(new Inventories.Commands.PreserveInventory.PreserveInventoryCommand(OrderId));
            if (result.IsSuccessfully)
            {
                Console.WriteLine("Update inventory for order {0} successfully", OrderId);
                await context.Publish<InventoryPreservedEvent>(new InventoryPreservedEvent(OrderId));
            }
            else
            {
                Console.WriteLine("Update inventory for order {0} failed", OrderId);
            }
        }
    }

    public class PreserveInventoryConsumerDefinition : ConsumerDefinition<PreserveInventoryConsumer>
    {
        public PreserveInventoryConsumerDefinition(IOptions<CommandConsumerEndpoints> config)
        {
            // override the default endpoint name, for whatever reason
            EndpointName = config.Value.PreserveInventoryConsumer;
        }

    }
}
