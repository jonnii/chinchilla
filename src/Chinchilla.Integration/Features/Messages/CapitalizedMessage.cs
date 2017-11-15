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
            Capitalized = result;
        }

        public Guid CorrelationId { get; set; }

        public string Capitalized { get; set; }
    }
}