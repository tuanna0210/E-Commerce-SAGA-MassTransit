namespace ECommerceSAGA.Configurations
{
    public class CommandConsumerEndpoints
    {
        public string ProcessPaymentConsumer { get; set; } = null!;
        public string PreserveInventoryConsumer { get; set; } = null!;
        public string ScheduleDeliveryConsumer { get; set; } = null!;
    }
}
