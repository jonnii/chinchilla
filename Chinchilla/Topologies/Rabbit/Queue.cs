using System;

namespace Chinchilla.Topologies.Rabbit
{
    public class Queue : Bindable, IQueue
    {
        public Queue()
        {
            Durability = Durability.Transient;
            IsAutoDelete = true;
        }

        public Queue(string name)
        {
            Name = name;
            Durability = Durability.Durable;
            IsAutoDelete = false;
        }

        public string Name { get; set; }

        public Durability Durability { get; set; }

        public bool IsAutoDelete { get; set; }

        public TimeSpan MessageTimeToLive { get; set; }

        public TimeSpan QueueAutoExpire { get; set; }

        public bool HasName
        {
            get { return !string.IsNullOrEmpty(Name); }
        }

        public override void Visit(ITopologyVisitor visitor)
        {
            visitor.Visit(this);

            foreach (var binding in Bindings)
            {
                visitor.Visit(binding);
            }
        }

        public override string ToString()
        {
            return string.Format("[Queue Name: {0}]", Name);
        }
    }
}