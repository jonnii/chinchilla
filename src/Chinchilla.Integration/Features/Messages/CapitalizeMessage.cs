using System;

namespace Chinchilla.Integration.Features.Messages
{
    public class CapitalizeMessage : ICorrelated
    {
        public CapitalizeMessage()
        {
            CorrelationId = Guid.NewGuid();
        }

        public CapitalizeMessage(string word)
            : this()
        {
            Word = word;
        }

        public Guid CorrelationId { get; set; }

        public string Word { get; set; }
    }
}
