namespace Chinchilla.Api
{
    public class Permission
    {
        public static Permission All
        {
            get { return new Permission(".*", ".*", ".*"); }
        }

        public Permission() { }

        public Permission(string configure, string write, string read)
        {
            Configure = configure;
            Write = write;
            Read = read;
        }

        public string Configure { get; set; }

        public string Write { get; set; }

        public string Read { get; set; }

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

            return obj.GetType() == GetType() && Equals((Permission)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Configure != null ? Configure.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (Write != null ? Write.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Read != null ? Read.GetHashCode() : 0);
                return hashCode;
            }
        }

        protected bool Equals(Permission other)
        {
            return string.Equals(Configure, other.Configure)
                   && string.Equals(Write, other.Write)
                   && string.Equals(Read, other.Read);
        }
    }
}