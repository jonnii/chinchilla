namespace Chinchilla
{
    public class MessageRejectedException : ChinchillaException
    {
        public MessageRejectedException()
        {
            ShouldRequeue = true;
        }

        public bool ShouldRequeue { get; set; }
    }
}
