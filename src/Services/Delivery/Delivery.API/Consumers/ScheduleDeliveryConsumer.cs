using BuildingBlocks.Messaging.Commands;
using BuildingBlocks.Messaging.Events;
using MassTransit;
using MediatR;

namespace Delivery.API.Consumers
{
    public class ScheduleDeliveryConsumer(ISender sender) : IConsumer<ScheduleDeliveryCommand>
    {
        public async Task Consume(ConsumeContext<ScheduleDeliveryCommand> context)
        {
            Guid OrderId = context.Message.OrderId;
            Console.WriteLine("ScheduleDeliveryConsumer For Order {0}", OrderId);
            var result = await sender.Send(new Deliveries.Commands.ScheduleDeliveryCommand(OrderId));
            if (result.IsSuccessfully)
            {
                Console.WriteLine("Schedule delivery for order {0} successfully", OrderId);
                await context.Publish<DeliveryCreatedEvent>(new DeliveryCreatedEvent(OrderId));
            }
            else
            {
                Console.WriteLine("Schedule delivery for order {0} failed", OrderId);
            }
        }
    }
}
