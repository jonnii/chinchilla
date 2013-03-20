using System;

namespace Chinchilla.Specifications.Messages
{
    public class TestRequestMessage : ICorrelated, IHasTimeOut
    {
        public TestRequestMessage()
        {
            CorrelationId = Guid.NewGuid();
        }

        public Guid CorrelationId { get; set; }

        public TimeSpan Timeout
        {
            get { return TimeSpan.FromSeconds(60); }
        }
    }
}
