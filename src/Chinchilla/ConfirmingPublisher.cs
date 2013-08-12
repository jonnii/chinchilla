using Chinchilla.Topologies.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Chinchilla
{
    public class ConfirmingPublisher<TMessage> : Publisher<TMessage>
    {
        private readonly IPublishFaultStrategy<TMessage> publishFaultStrategy;

        private readonly Receipts<TMessage> receipts = new Receipts<TMessage>();

        public ConfirmingPublisher(
            IModelReference modelReference,
            IMessageSerializer serializer,
            IExchange exchange,
            IRouter router,
            IPublishFaultStrategy<TMessage> publishFaultStrategy)
            : base(modelReference, serializer, exchange, router)
        {
            this.publishFaultStrategy = publishFaultStrategy;
        }

        public override void Start()
        {
            ModelReference.OnReconnect((oldModel, newModel) =>
            {
                oldModel.BasicAcks -= OnBasicAcks;
                oldModel.BasicNacks -= OnBasicNacks;

                newModel.ConfirmSelect();

                newModel.BasicAcks += OnBasicAcks;
                newModel.BasicNacks += OnBasicNacks;
            });

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

        public void OnBasicAcks(IModel model, BasicAckEventArgs args)
        {
            receipts.ProcessReceipts(args.Multiple, args.DeliveryTag, receipt => receipt.Confirmed());
        }

        public void OnBasicNacks(IModel model, BasicNackEventArgs args)
        {
            receipts.ProcessReceipts(args.Multiple, args.DeliveryTag, receipt =>
            {
                // Extract the failed message
                var failedMessage = receipt.Message;

                // Mark this receipt as failed
                receipt.Failed();

                // And ask the publish fault strategy what to do with this
                publishFaultStrategy.Run(this, failedMessage, receipt);
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
