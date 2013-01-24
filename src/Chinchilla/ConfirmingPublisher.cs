using System;
using System.Collections.Concurrent;
using System.Linq;
using Chinchilla.Topologies.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Chinchilla
{
    public class ConfirmingPublisher<TMessage> : Publisher<TMessage>
    {
        private readonly ConcurrentDictionary<ulong, ConfirmReceipt> receipts
            = new ConcurrentDictionary<ulong, ConfirmReceipt>();

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
            var receipt = CreateAndRegisterReceipt(
                model.NextPublishSeqNo);

            model.BasicPublish(
                Exchange.Name,
                routingKey,
                defaultProperties,
                serializedMessage);

            return receipt;
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

        private void OnBasicAcks(IModel model, BasicAckEventArgs args)
        {
            ProcessReceipts(args.Multiple, args.DeliveryTag, r => r.Confirmed());
        }

        private void OnBasicNacks(IModel model, BasicNackEventArgs args)
        {
            ProcessReceipts(args.Multiple, args.DeliveryTag, r => r.Failed());
        }

        public ConfirmReceipt CreateAndRegisterReceipt(ulong nextPublishSeqNo)
        {
            var receipt = new ConfirmReceipt(nextPublishSeqNo);
            RegisterReceipt(receipt);
            return receipt;
        }

        public ConfirmReceipt RegisterReceipt(ConfirmReceipt receipt)
        {
            if (receipts.TryAdd(receipt.Sequence, receipt))
            {
                return receipt;
            }

            var message = string.Format(
                "Could not register a confirm receipt because a " +
                "receipt for the sequence number {0} has already been registered",
                receipt.Sequence);

            throw new DuplicatePublishReceiptException(message);
        }

        public bool HasPendingConfirmation(ulong sequenceNumber)
        {
            return receipts.ContainsKey(sequenceNumber);
        }

        public void ProcessReceipts(bool multiple, ulong sequenceNumber, Action<ConfirmReceipt> act)
        {
            if (multiple)
            {
                var affected = receipts.Where(r => r.Key <= sequenceNumber).ToArray();

                foreach (var receipt in affected)
                {
                    ProcessReceipt(receipt.Key, act);
                }
            }
            else
            {
                ProcessReceipt(sequenceNumber, act);
            }
        }

        private void ProcessReceipt(ulong sequenceNumber, Action<ConfirmReceipt> act)
        {
            ConfirmReceipt receipt;
            if (receipts.TryRemove(sequenceNumber, out receipt))
            {
                act(receipt);
            }
        }
    }
}
