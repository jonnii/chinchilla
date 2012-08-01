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
    }
}