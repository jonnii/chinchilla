namespace Chinchilla
{
    public class NullReceipt : IPublishReceipt
    {
        public readonly static NullReceipt Instance = new NullReceipt();

        public PublishStatus Status
        {
            get { return PublishStatus.None; }
        }

        public bool IsConfirmed
        {
            get { return false; }
        }

        public PublishFailureReason FailureReason
        {
            get { return PublishFailureReason.None; }
        }
    }
}