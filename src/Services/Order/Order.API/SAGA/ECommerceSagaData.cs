using MassTransit;

namespace Order.API.SAGA
{
    /// <summary>
    /// State machine instance class, that gets persisted to disk
    /// </summary>
    public class ECommerceSagaData : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime OrderCreatedDate { get; set; }
        public DateTime OrderPaidDate { get; set; }
        public DateTime InventoryPreservedDate { get; set; }
        public DateTime DeliveryCreatedDate { get; set; }
        public DateTime OrderCompletedDate { get; set; }
    }
}
