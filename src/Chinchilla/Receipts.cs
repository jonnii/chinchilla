using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Chinchilla
{
    public class Receipts<TMessage>
    {
        private readonly ConcurrentDictionary<ulong, ConfirmReceipt<TMessage>> receipts
            = new ConcurrentDictionary<ulong, ConfirmReceipt<TMessage>>();

        public ConfirmReceipt<TMessage> CreateAndRegisterReceipt(ulong nextPublishSeqNo, TMessage message)
        {
            var receipt = new ConfirmReceipt<TMessage>(nextPublishSeqNo, message);
            RegisterReceipt(receipt);
            return receipt;
        }

        public ConfirmReceipt<TMessage> RegisterReceipt(ConfirmReceipt<TMessage> receipt)
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

        public void ProcessReceipts(bool multiple, ulong sequenceNumber, Action<ConfirmReceipt<TMessage>> act)
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

        public void ProcessAllReceipts(Action<ConfirmReceipt<TMessage>> act)
        {
            foreach (var receipt in receipts)
            {
                ProcessReceipt(receipt.Key, act);
            }
        }

        private void ProcessReceipt(ulong sequenceNumber, Action<ConfirmReceipt<TMessage>> act)
        {
            ConfirmReceipt<TMessage> receipt;
            if (receipts.TryRemove(sequenceNumber, out receipt))
            {
                act(receipt);
            }
        }
    }
}