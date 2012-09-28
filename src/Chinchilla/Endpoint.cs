namespace Chinchilla
{
    public class Endpoint : IEndpoint
    {
        public Endpoint(
            string endpointName,
            string messageType,
            int ordinal)
        {
            Name = endpointName;
            MessageType = messageType;
            Ordinal = ordinal;
        }

        public string Name { get; private set; }

        public string MessageType { get; private set; }

        public int Ordinal { get; private set; }
    }
}
