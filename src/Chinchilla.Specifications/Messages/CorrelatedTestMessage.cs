using System;

namespace Chinchilla.Specifications.Messages
{
    public class CorrelatedTestMessage : ICorrelated
    {
        public CorrelatedTestMessage()
        {
            CorrelationId = Guid.NewGuid();
        }

        public Guid CorrelationId { get; set; }
    }
}
