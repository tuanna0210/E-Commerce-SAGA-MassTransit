using BuildingBlocks.CQRS;
using Mapster;
using Order.API.Dtos;
using Order.API.Models;

namespace Order.API.Orders.CreateOrder
{
    public record CreateOrderCommand(CreateOrderDto Order) : ICommand<CreateOrderResult>;
    public record CreateOrderResult(Guid OrderId);
    public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, CreateOrderResult>
    {
        private readonly ApplicationDbContext _context;
        public CreateOrderCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<CreateOrderResult> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = request.Order.Adapt<Order.API.Models.Order>();
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return new CreateOrderResult(order.Id);
        }
    }
}
