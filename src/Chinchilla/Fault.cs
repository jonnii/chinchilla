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

        public string Exception { get; set; }

        public string Message { get; set; }

        public DateTime OccuredAt { get; set; }
    }
}