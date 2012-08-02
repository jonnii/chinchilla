namespace Chinchilla.Api
{
    public class VirtualHost
    {
        public VirtualHost() { }

        public VirtualHost(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}