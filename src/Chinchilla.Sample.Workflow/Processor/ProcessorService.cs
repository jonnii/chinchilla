using System;
using Chinchilla.Sample.Workflow.Messages;

namespace Chinchilla.Sample.Workflow.Processor
{
    public class ProcessorService
    {
        private readonly IBus bus;

        private ISubscription subscriber;

        private IPublisher<ProcessingJobUpdatedMessage> publisher;

        public ProcessorService()
        {
            bus = Depot.Connect("localhost/workflow");
        }

        public void Start()
        {
            publisher = bus.CreatePublisher<ProcessingJobUpdatedMessage>();

            subscriber = bus.Subscribe<MoveToNextWorkflowStateMessage>(m =>
            {
                Console.WriteLine("[Processor] Processing {0}", m.JobId);
                ProcessJob(m.JobId);
            });
        }

        private void ProcessJob(string jobId)
        {
            publisher.Publish(new ProcessingJobUpdatedMessage
            {
                ProcessingJobId = jobId,
                WorkflowState = "Completed",
                LogMessage = "The job was successfully completed, hooray!"
            });
        }

        public void Stop()
        {
            publisher.Dispose();
            subscriber.Dispose();
            bus.Dispose();
        }
    }
}
