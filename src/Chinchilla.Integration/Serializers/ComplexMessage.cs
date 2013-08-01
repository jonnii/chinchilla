namespace Chinchilla.Integration.Serializers
{
    public class ComplexMessage
    {
        public ComplexMessage()
        {

        }

        public ComplexMessage(string simpleString)
        {
            SimpleString = simpleString;
        }

        public string SimpleString { get; set; }
    }
}