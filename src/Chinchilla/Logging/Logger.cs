namespace Chinchilla.Logging
{
    public static class Logger
    {
        static Logger()
        {
            Factory = new NullLoggerFactory();
        }

        public static ILoggerFactory Factory { get; set; }

        public static ILogger Create<T>()
        {
            return Factory.GetLogger<T>();
        }
    }
}