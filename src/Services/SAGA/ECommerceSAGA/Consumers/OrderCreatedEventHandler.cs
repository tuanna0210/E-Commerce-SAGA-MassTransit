using BuildingBlocks.Messaging.Events;
using MassTransit;

namespace ECommerceSAGA.Consumers
{
    public class OrderCreatedEventHandler : IConsumer<OrderCreatedEvent>
    {
        public Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var orderId = context.Message.OrderId;
            return Task.CompletedTask;
        }
    }
}
