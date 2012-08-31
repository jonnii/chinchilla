using System;

namespace Chinchilla
{
    public class Fault : IHasRoutingKey
    {
        public Fault()
        {
            OccuredAt = DateTime.UtcNow;
        }

        public string RoutingKey { get; set; }

        public string Exchange { get; set; }

        public FaultException Exception { get; set; }

        public string FaultedMessage { get; set; }

        public DateTime OccuredAt { get; set; }
    }
}