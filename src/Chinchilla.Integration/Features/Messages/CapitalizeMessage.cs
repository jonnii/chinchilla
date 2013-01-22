namespace Chinchilla.Integration.Features.Messages
{
    public class CapitalizeMessage
    {
        public CapitalizeMessage() { }

        public CapitalizeMessage(string word)
        {
            Word = word;
        }

        public string Word { get; set; }
    }
}
