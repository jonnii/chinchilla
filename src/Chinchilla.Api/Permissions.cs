namespace Chinchilla.Api
{
    public class Permissions : Permission
    {
        public string Vhost { get; set; }

        public string User { get; set; }

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

            return obj.GetType() == GetType() && Equals((Permissions)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ (Vhost != null ? Vhost.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (User != null ? User.GetHashCode() : 0);
                return hashCode;
            }
        }

        protected bool Equals(Permissions other)
        {
            return base.Equals(other) && string.Equals(Vhost, other.Vhost) && string.Equals(User, other.User);
        }
    }
}