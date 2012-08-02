namespace Chinchilla.Logging
{
    public static class Logger
    {
        static Logger()
        {
            Target = new NullLogger();
        }

        public static ILogger Target { get; set; }

        public static ILogger Create<T>()
        {
            return new ClassLogger<T>();
        }
    }
}