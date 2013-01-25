using System;

namespace Chinchilla
{
    public class TaskWorker : Worker
    {
        private readonly string name;

        public TaskWorker(IDeliveryProcessor connectedProcessor)
            : base(connectedProcessor)
        {
            name = Guid.NewGuid().ToString();
        }

        public override string Name
        {
            get { return name; }
        }

        public override string Type
        {
            get { return "TaskWorker"; }
        }
    }
}