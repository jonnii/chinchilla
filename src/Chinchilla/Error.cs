using System;

namespace Chinchilla
{
    public class Error
    {
        public string RoutingKey { get; set; }

        public string Exchange { get; set; }

        public string Exception { get; set; }

        public string Message { get; set; }

        public DateTime DateTime { get; set; }
    }
}