namespace Chinchilla.Api
{
    public class Exchange
    {
        public Exchange() { }

        public Exchange(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        protected bool Equals(Exchange other)
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

            return obj.GetType() == GetType() && Equals((Exchange)obj);
        }

        public override int GetHashCode()
        {
            return Name != null ? Name.GetHashCode() : 0;
        }
    }
}