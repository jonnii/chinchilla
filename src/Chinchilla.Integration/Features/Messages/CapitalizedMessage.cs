using System;

namespace Chinchilla.Integration.Features.Messages
{
    public class CapitalizedMessage : ICorrelated
    {
        public CapitalizedMessage()
        {
            CorrelationId = Guid.NewGuid();
        }

        public CapitalizedMessage(string result)
            : this()
        {
            Result = result;
        }

        public Guid CorrelationId { get; private set; }

        public string Result { get; set; }
    }
}