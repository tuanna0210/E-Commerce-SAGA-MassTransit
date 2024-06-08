using BuildingBlocks.CQRS;

namespace Delivery.API.Deliveries.Commands;

public record ScheduleDeliveryCommand(Guid OdderId) : ICommand<ScheduleDeliveryResult>;
public record ScheduleDeliveryResult(bool IsSuccessfully);
public class ScheduleDeliveryCommandHandler : ICommandHandler<ScheduleDeliveryCommand, ScheduleDeliveryResult>
{
    public async Task<ScheduleDeliveryResult> Handle(ScheduleDeliveryCommand request, CancellationToken cancellationToken)
    {
        //TODO, schedule delivery for order: create delivery record, assign shipment services,...
        return new ScheduleDeliveryResult(true);
    }
}
