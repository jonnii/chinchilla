namespace Chinchilla.Api
{
    public class User
    {
        public User() { }

        public User(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}