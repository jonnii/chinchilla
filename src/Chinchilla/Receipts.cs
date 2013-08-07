using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Chinchilla
{
    public class Receipts
    {
        private readonly ConcurrentDictionary<ulong, ConfirmReceipt> receipts
            = new ConcurrentDictionary<ulong, ConfirmReceipt>();

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