namespace Chinchilla.Sample.StockTicker.Messages
{
    public class ConnectMessage
    {
        public ConnectMessage() { }

        public ConnectMessage(string clientId, string queueName, string[] tickers)
        {
            ClientId = clientId;
            Tickers = tickers;
            QueueName = queueName;
        }

        public string ClientId { get; set; }

        public string QueueName { get; set; }

        public string[] Tickers { get; set; }
    }
}
