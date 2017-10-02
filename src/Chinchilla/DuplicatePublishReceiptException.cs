using System;

namespace Chinchilla
{
    public class DuplicatePublishReceiptException : Exception
    {
        public DuplicatePublishReceiptException() { }

        public DuplicatePublishReceiptException(string message) : base(message) { }

        public DuplicatePublishReceiptException(string message, Exception inner) : base(message, inner) { }
    }
}