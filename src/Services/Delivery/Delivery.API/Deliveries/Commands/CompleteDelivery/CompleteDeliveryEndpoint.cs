using Carter;
using Mapster;
using MediatR;

namespace Delivery.API.Deliveries.Commands.CompleteDelivery;

public record CompleteDeliveryRequest(Guid OrderId, string DeliveredBy);
public class CompleteDeliveryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/deliveries/complete", async (CompleteDeliveryRequest request, ISender sender) =>
        {
            var command = request.Adapt<CompleteDeliveryCommand>();
            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .WithDescription("Complete delivery for order")
        .WithName("CompleteDelivery")
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
