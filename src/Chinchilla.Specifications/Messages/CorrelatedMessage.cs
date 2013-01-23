using System;

namespace Chinchilla.Specifications.Messages
{
    public class CorrelatedMessage : ICorrelated
    {
        public Guid CorrelationId { get; set; }
    }
}
