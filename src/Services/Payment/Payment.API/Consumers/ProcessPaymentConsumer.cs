using BuildingBlocks.Messaging.Commands;
using BuildingBlocks.Messaging.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Options;
using Payment.API.Contracts.Configurations;

namespace Payment.API.Consumers
{
    public class ProcessPaymentConsumer(ISender sender) : IConsumer<ProcessPaymentCommand>
    {
        public async Task Consume(ConsumeContext<ProcessPaymentCommand> context)
        {
            Guid OrderId = context.Message.OrderId;
            Console.WriteLine("ProcessPaymentConsumer For Order {0}", OrderId);
            var result = await sender.Send(new Payments.Commands.ProcessPayment.ProcessPaymentCommand(OrderId));
            if (result.IsSuccessfully)
            {
                Console.WriteLine("Process payment for order {0} successfully", OrderId);
                await context.Publish<PaymentSuceededEvent>(new PaymentSuceededEvent()
                {
                    OrderId = OrderId
                });
            }
            else
            {
                Console.WriteLine("Process payment for order {0} failed", OrderId);

                //TODO:Send mail inform user to try to process payment again?

                await context.Publish<PaymentFailedEvent>(new PaymentFailedEvent()
                {
                    OrderId = OrderId
                });
            }
        }
    }

    public class ProcessPaymentConsumerDefinition : ConsumerDefinition<ProcessPaymentConsumer>
    {
        public ProcessPaymentConsumerDefinition(IOptions<CommandConsumerEndpoints> config)
        {
            // override the default endpoint name, for whatever reason
            EndpointName = config.Value.ProcessPaymentConsumer;
        }

    }
}
