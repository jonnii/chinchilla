using System;

namespace Chinchilla.Logging
{
    public class ConsoleLogger : ILogger
    {
        public void Info(string message)
        {
            Console.WriteLine("INFO: " + message);
        }

        public void InfoFormat(string format, params object[] args)
        {
            Console.WriteLine("INFO: " + format, args);
        }

        public void Debug(string message)
        {
            Console.WriteLine("DEBUG: " + message);
        }

        public void DebugFormat(string format, params object[] args)
        {
            Console.WriteLine("DEBUG: " + format, args);
        }
    }
}
