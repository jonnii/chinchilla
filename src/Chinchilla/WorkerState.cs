namespace Chinchilla
{
    /// <summary>
    /// A delivery strategy state contains all the details of what the message
    /// delivery system is currently doing
    /// </summary>
    public class WorkerState
    {
        public WorkerState(
            string type,
            WorkerStatus status)
        {
            Type = type;
            Status = status;
        }

        /// <summary>
        /// The type of this worker
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The status of this worker
        /// </summary>
        public WorkerStatus Status { get; private set; }
    }
}