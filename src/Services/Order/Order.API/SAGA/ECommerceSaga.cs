using BuildingBlocks.Messaging.Commands;
using BuildingBlocks.Messaging.Events;
using BuildingBlocks.Messaging.Messages;
using MassTransit;
using Microsoft.Extensions.Options;
using Order.API.Contracts.Configurations;

namespace Order.API.SAGA
{
    public class ECommerceSaga : MassTransitStateMachine<ECommerceSagaData>
    {
        //States
        public State OrderCreated { get; set; }
        public State PaymentProcessing { get; set; }
        public State OrderPaid { get; set; }
        public State OrderPaymentFailed { get; set; }
        public State InventoryPreserving { get; set; }

        public State InventoryPreserved { get; set; }
        public State PreserveProductsFailed { get; set; }
        public State OrderDelivering { get; set; }
        public State OrderCompleted { get; set; }

        //Events
        public Event<OrderCreatedEvent> OrderCreatedEvent { get; set; }
        public Event<PaymentSuceededEvent> PaymentSuceededEvent { get; set; }
        public Event<PaymentFailedEvent> PaymentFailedEvent { get; set; }

        public Event<InventoryPreservedEvent> InventoryPreservedEvent { get; set; }
        //public Event<PreserveProductsFailedEvent> PreserveProductsFailedEvent { get; set; }
        public Event<DeliveryCreatedEvent> DeliveryCreatedEvent { get; set; }
        public Event<DeliveryCompletedEvent> DeliveryCompletedEvent { get; set; }

        public ECommerceSaga(IOptions<CommandConsumerEndpoints> config)
        {
            var endpointAddresses = config.Value;
            //var scheduleDeliveryEndpoint = new Uri($"queue:{KebabCaseEndpointNameFormatter.Instance.Message<ScheduleDeliveryCommand>()}");

            //Correlate the event on the state machine
            //Mean the OrderId from the event is going to be used to correlate to the saga instance
            //And if the saga instance does not exist in the repo, MassTransit is going to create a new ơn for us
            Event(() => OrderCreatedEvent, e => e.CorrelateById(m => m.Message.OrderId));

            Event(() => PaymentSuceededEvent, e => e.CorrelateById(m => m.Message.OrderId));
            Event(() => PaymentFailedEvent, e => e.CorrelateById(m => m.Message.OrderId));

            Event(() => InventoryPreservedEvent, e => e.CorrelateById(m => m.Message.OrderId));
            Event(() => DeliveryCreatedEvent, e => e.CorrelateById(m => m.Message.OrderId));
            Event(() => DeliveryCompletedEvent, e => e.CorrelateById(m => m.Message.OrderId));
            //Event(() => PreserveProductsFailedEvent, e => e.CorrelateById(m => m.Message.OrderId));

            //Set the current state for state machine by the saga instance's current state
            InstanceState(x => x.CurrentState);

            //Behaviors
            //TODO: Update order status when the stage changed
            //TODO: Ignore invalid events for certain state (ex: during OrderPaid => Ignore incoming event PaymentSuceededEvent, or we can config OnMissingInstance on events
            Initially(
                When(OrderCreatedEvent)
                    .Then(context =>
                    {
                        context.Saga.OrderCreatedDate = DateTime.UtcNow;
                        context.Saga.CustomerId = context.Message.CustomerId; //Copy customerId from the incoming message (OrderCreatedEvent) to saga instance
                    })
                    .TransitionTo(PaymentProcessing)
                    //.Publish(x => new ProcessPaymentCommand()
                    //{
                    //    OrderId = x.Message.OrderId
                    //})
                    .Send(new Uri($"queue:{endpointAddresses.ProcessPaymentConsumer}"), x => new ProcessPaymentCommand()//Using Send activity because this is a command (one-to-one) not event(pub-sub)
                    {
                        OrderId = x.Message.OrderId
                    }));

            During(PaymentProcessing,
                When(PaymentSuceededEvent)
                    .Then(context =>
                    {
                        context.Saga.OrderPaidDate = DateTime.UtcNow;
                        Console.WriteLine("State machine transition from 'PaymentProcessing' to 'OrderPaid'");
                    })
                    .TransitionTo(OrderPaid)
                    .Send(new Uri($"queue:{endpointAddresses.PreserveInventoryConsumer}"), x => new UpdateInventoryCommand()
                    {
                        OrderId = x.Message.OrderId
                    }),
                When(PaymentFailedEvent)//Compensating flow
                    .Then(context =>
                    {
                        Console.WriteLine("State machine transition from 'PaymentProcessing' to 'OrderPaymentFailed'");
                    })
                    .TransitionTo(OrderPaymentFailed));
            //,
            //When(PreserveProductsFailedEvent)
            //.Then(context =>
            //{

            //})
            //.TransitionTo(PreserveProductsFailed));

            During(OrderPaid,
                Ignore(PaymentSuceededEvent),//When order is paid, there's no point of processing payment again
                When(InventoryPreservedEvent)
                    .Then(context =>
                    {
                        context.Saga.InventoryPreservedDate = DateTime.UtcNow;
                        Console.WriteLine("State machine transition from 'OrderPaid' to 'InventoryPreserved'");
                    })
                    .TransitionTo(InventoryPreserved)
                    .Send(new Uri($"queue:{endpointAddresses.ScheduleDeliveryConsumer}"), x => new ScheduleDeliveryCommand(x.Message.OrderId)));


            //When user process payment again
            During(OrderPaymentFailed,
                When(PaymentSuceededEvent)
                    .Then(context =>
                    {
                        context.Saga.OrderPaidDate = DateTime.UtcNow;
                        Console.WriteLine("State machine transition from 'PaymentProcessing' to 'OrderPaid'");
                    })
                    .TransitionTo(OrderPaid)
                    .Send(new Uri($"queue:{endpointAddresses.PreserveInventoryConsumer}"), x => new UpdateInventoryCommand()
                    {
                        OrderId = x.Message.OrderId
                    }));

            During(InventoryPreserved,
                When(DeliveryCreatedEvent)
                    .Then(context =>
                    {
                        context.Saga.DeliveryCreatedDate = DateTime.UtcNow;
                        Console.WriteLine("State machine transition from 'InventoryPreserved' to 'OrderDelivering'");
                    })
                    .TransitionTo(OrderDelivering));

            During(OrderDelivering,
                When(DeliveryCompletedEvent)
                    .Then(context =>
                    {
                        context.Saga.OrderCompletedDate = DateTime.UtcNow;
                        Console.WriteLine("State machine transition from 'OrderDelivering' to 'OrderCompleted'");
                    })
                    .TransitionTo(OrderCompleted)
                    .Finalize());

            //Set state complete đồng thời xóa instance khỏi reposirory (xóa khỏi persistence)
            //SetCompletedWhenFinalized();
        }
    }
}
