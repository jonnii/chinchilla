namespace Chinchilla
{
    /// <summary>
    /// A delivery strategy state contains all the details of what the message
    /// delivery system is currently doing
    /// </summary>
    public class WorkerState
    {
        public WorkerState(WorkerStatus status)
        {
            Status = status;
        }

        /// <summary>
        /// The status of this worker
        /// </summary>
        public WorkerStatus Status { get; private set; }
    }
}