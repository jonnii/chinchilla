using Chinchilla.Topologies.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Chinchilla
{
    public class ConfirmingPublisher<TMessage> : Publisher<TMessage>
    {
        private readonly Receipts receipts = new Receipts();

        public ConfirmingPublisher(
            IModelReference modelReference,
            IMessageSerializer serializer,
            IExchange exchange,
            IRouter router)
            : base(modelReference, serializer, exchange, router)
        {

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
            IModel model,
            string routingKey,
            IBasicProperties defaultProperties,
            byte[] serializedMessage)
        {
            var receipt = receipts.CreateAndRegisterReceipt(
                model.NextPublishSeqNo);

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

        private void OnBasicAcks(IModel model, BasicAckEventArgs args)
        {
            receipts.ProcessReceipts(args.Multiple, args.DeliveryTag, r => r.Confirmed());
        }

        private void OnBasicNacks(IModel model, BasicNackEventArgs args)
        {
            receipts.ProcessReceipts(args.Multiple, args.DeliveryTag, r => r.Failed());
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
