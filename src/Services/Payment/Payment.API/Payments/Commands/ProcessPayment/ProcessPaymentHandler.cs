using BuildingBlocks.CQRS;

namespace Payment.API.Payments.Commands.ProcessPayment;

public record ProcessPaymentCommand(Guid OrderId) : ICommand<ProcessPaymentResult>;
public record ProcessPaymentResult(bool IsSuccessfully);
public class ProcessPaymentCommandHandler : ICommandHandler<ProcessPaymentCommand, ProcessPaymentResult>
{
    public async Task<ProcessPaymentResult> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
    {
        //TODO, payment logic here
        var random = new Random();
        if (random.Next(1, 11) == 1)//Simulate payment failed for 1 out of 10 changes
        {
            return new ProcessPaymentResult(false);
        }
        else
        {
            return new ProcessPaymentResult(true);
        }

    }
}
