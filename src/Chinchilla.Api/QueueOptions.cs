namespace Chinchilla.Api
{
    public class QueueOptions
    {
        public static QueueOptions Default
        {
            get { return new QueueOptions(); }
        }

        public bool AutoDelete { get; set; }

        public bool Durable { get; set; }
    }
}
