using System;

namespace Chinchilla
{
    /// <summary>
    /// A delivery strategy state contains all the details of what the message
    /// delivery system is currently doing
    /// </summary>
    public class WorkerState
    {
        public WorkerState(string name, string type, WorkerStatus status, DateTime? busySince)
        {
            Name = name;
            Type = type;
            Status = status;
            BusySince = busySince;
        }

        /// <summary>
        /// The name of this worker
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The type of this worker
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The status of this worker
        /// </summary>
        public WorkerStatus Status { get; private set; }

        /// <summary>
        /// Indicates when this worker state last entered the busy state
        /// </summary>
        public DateTime? BusySince { get; set; }
    }
}