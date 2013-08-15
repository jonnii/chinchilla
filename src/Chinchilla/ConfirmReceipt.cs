namespace Chinchilla
{
    public class ConfirmReceipt
    {
        public static ConfirmReceipt<TMessage> New<TMessage>(ulong sequence, TMessage message)
        {
            return new ConfirmReceipt<TMessage>(sequence, message);
        }
    }

    public class ConfirmReceipt<TMessage> : IPublishReceipt
    {
        public ConfirmReceipt(ulong sequence, TMessage message)
        {
            Message = message;
            Status = PublishStatus.Pending;
            Sequence = sequence;
        }

        public TMessage Message { get; private set; }

        public PublishStatus Status { get; private set; }

        public PublishFailureReason FailureReason { get; private set; }

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
            Message = default(TMessage);
        }

        public void Failed(PublishFailureReason reason)
        {
            Status = PublishStatus.Failed;
            FailureReason = reason;
            Message = default(TMessage);
        }
    }
}