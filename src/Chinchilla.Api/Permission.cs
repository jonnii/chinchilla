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
    }
}