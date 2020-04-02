namespace Ilvo.DataHub.Samples.Provider.Models
{
    public class Event
    {
        public string Topic { get; set; }
        public string EventType { get; set; }
        public string Subject { get; set; }
        public string Key { get; set; }
        public object Payload { get; set; }
    }
}
