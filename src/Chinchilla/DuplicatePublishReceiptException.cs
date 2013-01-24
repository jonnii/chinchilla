using System;
using System.Runtime.Serialization;

namespace Chinchilla
{
    public class DuplicatePublishReceiptException : Exception
    {
        public DuplicatePublishReceiptException() { }

        public DuplicatePublishReceiptException(string message) : base(message) { }

        public DuplicatePublishReceiptException(string message, Exception inner) : base(message, inner) { }

        protected DuplicatePublishReceiptException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}