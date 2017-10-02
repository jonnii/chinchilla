using System;

namespace Chinchilla
{
    public class ChinchillaException : Exception
    {
        public ChinchillaException() { }

        public ChinchillaException(string message) : base(message) { }

        public ChinchillaException(string message, Exception inner) : base(message, inner) { }
    }
}