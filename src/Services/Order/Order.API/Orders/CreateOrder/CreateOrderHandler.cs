using BuildingBlocks.CQRS;
using BuildingBlocks.Messaging.Events;
using Mapster;
using MassTransit;
using Order.API.Dtos;
using Order.API.Models;

namespace Order.API.Orders.CreateOrder
{
    public record CreateOrderCommand(CreateOrderDto Order) : ICommand<CreateOrderResult>;
    public record CreateOrderResult(Guid OrderId);
    public class CreateOrderCommandHandler(
        ApplicationDbContext context,
        IPublishEndpoint publishEndpoint
        ) : ICommandHandler<CreateOrderCommand, CreateOrderResult>
    {
        public async Task<CreateOrderResult> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = request.Order.Adapt<Order.API.Models.Order>();
            context.Orders.Add(order);
            await context.SaveChangesAsync(cancellationToken);

            await publishEndpoint.Publish<OrderCreatedEvent>(new OrderCreatedEvent
            {
                OrderId = order.Id,
                CustomerId = order.CustomerId
            }, cancellationToken);

            return new CreateOrderResult(order.Id);
        }
    }
}
