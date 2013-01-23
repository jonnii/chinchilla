using System;

namespace Chinchilla.Specifications.Messages
{
    public class TestResponseMessage : ICorrelated
    {
        public TestResponseMessage()
        {
            CorrelationId = Guid.NewGuid();
        }

        public Guid CorrelationId { get; set; }
    }
}