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
    }
}