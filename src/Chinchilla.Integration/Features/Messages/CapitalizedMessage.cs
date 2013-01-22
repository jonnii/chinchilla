namespace Chinchilla.Integration.Features.Messages
{
    public class CapitalizedMessage
    {
        public CapitalizedMessage() { }

        public CapitalizedMessage(string result)
        {
            Result = result;
        }

        public string Result { get; set; }
    }
}