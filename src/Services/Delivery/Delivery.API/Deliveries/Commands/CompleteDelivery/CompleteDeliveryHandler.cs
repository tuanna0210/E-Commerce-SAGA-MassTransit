using BuildingBlocks.CQRS;
using BuildingBlocks.Messaging.Events;
using MassTransit;

namespace Delivery.API.Deliveries.Commands.CompleteDelivery
{
    public record CompleteDeliveryCommand(Guid OrderId, string DeliveredBy) : ICommand<CompleteDeliveryResult>;
    public record CompleteDeliveryResult(bool IsSuccessfully);
    public class CompleteDeliveryCommandHandler(IPublishEndpoint publishEndpoint) : ICommandHandler<CompleteDeliveryCommand, CompleteDeliveryResult>
    {
        public async Task<CompleteDeliveryResult> Handle(CompleteDeliveryCommand request, CancellationToken cancellationToken)
        {
            //TODO, complete delivery logic here
            await publishEndpoint.Publish<DeliveryCompletedEvent>(new DeliveryCompletedEvent()
            {
                OrderId = request.OrderId,
                DeliveredBy = request.DeliveredBy
            }, cancellationToken);

            return new CompleteDeliveryResult(true);
        }
    }
}
