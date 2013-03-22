namespace Chinchilla
{
    /// <summary>
    /// A workers controller is used to control the available workers on 
    /// a subscription
    /// </summary>
    public interface IWorkersController
    {
        /// <summary>
        /// Paused a worker
        /// </summary>
        void Pause(string workerName);

        /// <summary>
        /// Resumes a worker
        /// </summary>
        void Resume(string workerName);
    }
}