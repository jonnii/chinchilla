namespace Chinchilla.Sample.StockTicker.Messages
{
    public class PriceMessage : IHasRoutingKey
    {
        public PriceMessage() { }

        public PriceMessage(string ticker, int price)
        {
            Ticker = ticker;
            Price = price;
        }

        public string Ticker { get; set; }

        public int Price { get; set; }

        public string RoutingKey
        {
            get { return "prices." + Ticker; }
        }
    }
}
