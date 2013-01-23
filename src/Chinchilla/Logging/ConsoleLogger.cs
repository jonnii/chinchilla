using System;

namespace Chinchilla.Logging
{
    public class ConsoleLogger : ILogger
    {
        public void Info(string message)
        {
            Console.WriteLine("INFO:\t" + message);
        }

        public void InfoFormat(string format, params object[] args)
        {
            Console.WriteLine("INFO:\t" + format, args);
        }

        public void Debug(string message)
        {
            Console.WriteLine("DEBUG:\t" + message);
        }

        public void DebugFormat(string format, params object[] args)
        {
            Console.WriteLine("DEBUG:\t" + format, args);
        }

        public void Error(Exception exception)
        {
            Console.WriteLine(exception.Message);
            Console.WriteLine(exception.StackTrace);
        }

        public void Error(Exception exception, string message)
        {
            Console.WriteLine(message);
            Error(exception);
        }

        public void ErrorFormat(Exception exception, string format, params object[] args)
        {
            Console.WriteLine(format, args);
            Error(exception);
        }
    }
}
