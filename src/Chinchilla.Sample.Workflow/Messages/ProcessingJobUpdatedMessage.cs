namespace Chinchilla.Sample.Workflow.Messages
{
    public class ProcessingJobUpdatedMessage
    {
        public string ProcessingJobId { get; set; }

        public string WorkflowState { get; set; }

        public string LogMessage { get; set; }
    }
}
