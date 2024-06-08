using BuildingBlocks.Messaging.Events;
using Carter;
using MassTransit;
using MediatR;

namespace Payment.API.Payments.Commands.ProcessPayment
{
    public class ProcessPaymentEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/payment/order/{orderId}", async (Guid orderId, ISender sender, IPublishEndpoint publishEndpoint) =>
            {
                var command = new ProcessPaymentCommand(orderId);
                var result = await sender.Send(command);
                //TODO: is this logic belong here, or inside command handler
                if (result.IsSuccessfully)
                {
                    Console.WriteLine("Process payment for order {0} successfully", orderId);
                    await publishEndpoint.Publish<PaymentSuceededEvent>(new PaymentSuceededEvent()
                    {
                        OrderId = orderId
                    });
                }
                return Results.Ok(result);
            })
            .WithDescription("Process payment for order again, when the initial payment (during order submition) is failed");
        }
    }
}
