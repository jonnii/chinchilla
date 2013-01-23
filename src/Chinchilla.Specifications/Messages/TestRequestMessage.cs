using System;

namespace Chinchilla.Specifications.Messages
{
    public class TestRequestMessage : ICorrelated
    {
        public TestRequestMessage()
        {
            CorrelationId = Guid.NewGuid();
        }

        public Guid CorrelationId { get; set; }
    }
}
