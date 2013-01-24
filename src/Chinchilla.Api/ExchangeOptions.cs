namespace Chinchilla.Api
{
    public class ExchangeOptions
    {
        public static ExchangeOptions Default
        {
            get
            {
                return new ExchangeOptions("topic")
                {
                    Durable = true
                };
            }
        }

        public ExchangeOptions(string exchangeType)
        {
            Type = exchangeType;
        }

        public string Type { get; set; }

        public bool Durable { get; set; }
    }
}
