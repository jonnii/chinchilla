using System;
using Chinchilla.Configuration;
using Chinchilla.Sample.Workflow.Messages;

namespace Chinchilla.Sample.Workflow.Listener
{
    public class ListenerService
    {
        private readonly int listenerId;

        private readonly IBus bus;

        public ListenerService(int listenerId)
        {
            this.listenerId = listenerId;
            bus = Depot.Connect("localhost/workflow");
        }

        public void Start()
        {
            bus.Subscribe<ProcessingJobUpdatedMessage>(OnJobUpdatedMessage, ConfigureListener);
        }

        private void ConfigureListener(ISubscriptionBuilder builder)
        {
            builder.SetTopology<ListenerTopology>();
        }

        protected void OnJobUpdatedMessage(ProcessingJobUpdatedMessage message)
        {
            Console.WriteLine("Listener/{0}: [{1}] {2} :: {3}",
                listenerId,
                message.WorkflowState,
                message.ProcessingJobId,
                message.LogMessage);
        }

        public void Stop()
        {
            bus.Dispose();
        }
    }
}
