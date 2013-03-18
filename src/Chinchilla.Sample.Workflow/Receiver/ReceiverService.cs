using System;
using System.Threading;
using Chinchilla.Sample.Workflow.Messages;

namespace Chinchilla.Sample.Workflow.Receiver
{
    public class ReceiverService
    {
        private bool isRunning;

        private readonly IBus bus;

        public ReceiverService()
        {
            bus = Depot.Connect("localhost/workflow");
        }

        public void Start()
        {
            isRunning = true;

            using (var publisher = bus.CreatePublisher<MoveToNextWorkflowStateMessage>())
            {
                while (isRunning)
                {
                    Console.WriteLine("[Receiver] Starting new move to next workflow");
                    publisher.Publish(new MoveToNextWorkflowStateMessage { JobId = Guid.NewGuid().ToString() });
                    Thread.Sleep(5000);
                }
            }
        }

        public void Stop()
        {
            bus.Dispose();
            isRunning = false;
        }
    }
}
