using System;
using System.Collections.Generic;
using System.Threading;
using Chinchilla.Sample.Workflow.Listener;
using Chinchilla.Sample.Workflow.Processor;
using Chinchilla.Sample.Workflow.Receiver;

namespace Chinchilla.Sample.Workflow
{
    public class WorkflowSample : IDisposable
    {
        private readonly ReceiverService receiver = new ReceiverService();

        private readonly ProcessorService processor = new ProcessorService();

        private readonly List<ListenerService> listenerServices = new List<ListenerService>();

        public void Run()
        {
            var receiverThread = new Thread(() => receiver.Start());
            receiverThread.Start();

            var processorThread = new Thread(() => processor.Start());
            processorThread.Start();

            for (var i = 0; i < 3; ++i)
            {
                Console.WriteLine("Starting listener: {0}", i);

                var listener = new ListenerService(i);
                var listenerThread = new Thread(listener.Start);
                listenerThread.Start();
            }
        }

        public void Dispose()
        {
            receiver.Stop();
            processor.Stop();
            listenerServices.ForEach(s => s.Stop());
        }
    }
}