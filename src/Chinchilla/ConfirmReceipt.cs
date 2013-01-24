namespace Chinchilla
{
    public class ConfirmReceipt : IPublishReceipt
    {
        public ConfirmReceipt(ulong sequence)
        {
            Status = PublishStatus.Pending;
            Sequence = sequence;
        }

        public PublishStatus Status { get; private set; }

        public ulong Sequence { get; set; }

        public bool IsConfirmed
        {
            get { return Status == PublishStatus.Confirmed; }
        }

        public bool IsFailed
        {
            get { return Status == PublishStatus.Failed; }
        }

        public void Confirmed()
        {
            Status = PublishStatus.Confirmed;
        }

        public void Failed()
        {
            Status = PublishStatus.Failed;
        }
    }
}