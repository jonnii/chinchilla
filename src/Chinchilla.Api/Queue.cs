namespace Chinchilla.Api
{
    public class Queue
    {
        public Queue() { }

        public Queue(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        protected bool Equals(Queue other)
        {
            return string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((Queue)obj);
        }

        public override int GetHashCode()
        {
            return Name != null ? Name.GetHashCode() : 0;
        }
    }
}