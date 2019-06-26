namespace Core.Device.Configurations
{
    public class RabbitConfig
    {
        public string ExchangeQueueDevice { get; set; }
        public string InputQueueDevice { get; set; }
        public string OutputQueueDevice { get; set; }
        public string RoutingKeyInputDevice { get; set; }
        public string RoutingKeyOutputDevice { get; set; }
    }
}
