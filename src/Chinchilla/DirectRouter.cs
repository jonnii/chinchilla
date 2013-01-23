namespace Chinchilla
{
    public class DirectRouter : IRouter
    {
        private readonly string replyTo;

        public DirectRouter(string replyTo)
        {
            this.replyTo = replyTo;
        }

        public string Route<TMessage>(TMessage message)
        {
            return replyTo;
        }

        public string ReplyTo()
        {
            return string.Empty;
        }
    }
}