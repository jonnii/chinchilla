using System;

namespace Chinchilla.Sample.Timeouts.Messages
{
    public class TimeoutMessage : IHasTimeOut, ICorrelated
    {
        public TimeoutMessage()
        {
            CorrelationId = Guid.NewGuid();
        }

        public Guid CorrelationId { get; set; }

        public string JobId { get; set; }

        public TimeSpan Timeout
        {
            get { return TimeSpan.FromSeconds(2); }
        }
    }
}
