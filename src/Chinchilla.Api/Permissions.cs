namespace Chinchilla.Api
{
    public class Permissions
    {
        public static Permissions All
        {
            get { return new Permissions(".*", ".*", ".*"); }
        }

        public Permissions() { }

        public Permissions(string configure, string write, string read)
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