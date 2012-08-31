namespace Chinchilla.Api
{
    public class Message
    {
        public int PayloadBytes { get; set; }
        public bool Redelivered { get; set; }
        public string Exchange { get; set; }
        public string RoutingKey { get; set; }
        public int MessageCount { get; set; }
        public MessageProperties MessageProperties { get; set; }
        public string Payload { get; set; }
    }
}
