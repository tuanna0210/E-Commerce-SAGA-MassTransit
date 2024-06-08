using Carter;
using Mapster;
using MediatR;
using Order.API.Dtos;

namespace Order.API.Orders.CreateOrder;

public record CreateOrderRequest(Guid CustomerId);
public record CreateOrderResponse(Guid OrderId);

public class CreateOrderEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/orders", async (CreateOrderRequest request, ISender sender) =>
        {
            var command = new CreateOrderCommand(request.Adapt<CreateOrderDto>());
            var result = await sender.Send(command);

            var response = new CreateOrderResponse(result.OrderId);
            return Results.Ok(response);
        });
    }
}
