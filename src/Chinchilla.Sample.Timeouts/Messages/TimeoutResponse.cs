using System;

namespace Chinchilla.Sample.Timeouts.Messages
{
    public class TimeoutResponse : ICorrelated
    {
        public TimeoutResponse()
        {
            CorrelationId = Guid.NewGuid();
        }

        public Guid CorrelationId { get; set; }
    }
}