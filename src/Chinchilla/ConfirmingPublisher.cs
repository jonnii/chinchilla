using Chinchilla.Topologies.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Chinchilla
{
    public class ConfirmingPublisher<TMessage> : Publisher<TMessage>
    {
        private readonly IPublisherFailureStrategy<TMessage> publisherFailureStrategy;

        private readonly Receipts<TMessage> receipts = new Receipts<TMessage>();

        public ConfirmingPublisher(
            IModelReference modelReference,
            IMessageSerializer serializer,
            IExchange exchange,
            IRouter router,
            IPublisherFailureStrategy<TMessage> publisherFailureStrategy)
            : base(modelReference, serializer, exchange, router)
        {
            this.publisherFailureStrategy = publisherFailureStrategy;
        }

        public override void Start()
        {
            ModelReference.Execute(m => m.ConfirmSelect());
            ModelReference.Execute(m => m.BasicAcks += OnBasicAcks);
            ModelReference.Execute(m => m.BasicNacks += OnBasicNacks);
        }

        public override IPublishReceipt PublishWithReceipt(
            TMessage originalMessage,
            IModel model,
            string routingKey,
            IBasicProperties defaultProperties,
            byte[] serializedMessage)
        {
            var receipt = receipts.CreateAndRegisterReceipt(
                model.NextPublishSeqNo,
                originalMessage);

            model.BasicPublish(
                Exchange.Name,
                routingKey,
                defaultProperties,
                serializedMessage);

            return receipt;
        }

        public bool HasPendingConfirmation(ulong sequenceNumber)
        {
            return receipts.HasPendingConfirmation(sequenceNumber);
        }

        public void OnBasicAcks(object sender, BasicAckEventArgs args)
        {
            receipts.ProcessReceipts(args.Multiple, args.DeliveryTag, receipt => receipt.Confirmed());
        }

        public void OnBasicNacks(object sender, BasicNackEventArgs args)
        {
            receipts.ProcessReceipts(args.Multiple, args.DeliveryTag, receipt =>
            {
                // Extract the failed message
                var failedMessage = receipt.Message;

                // Mark this receipt as failed
                receipt.Failed(PublishFailureReason.Nack);

                // And ask the publisher fault strategy what to do with this
                publisherFailureStrategy.OnFailure(this, failedMessage, receipt);
            });
        }

        public override void Dispose()
        {
            if (disposed)
            {
                return;
            }

            ModelReference.Execute(m => m.WaitForConfirmsOrDie());

            base.Dispose();
        }
    }
}
