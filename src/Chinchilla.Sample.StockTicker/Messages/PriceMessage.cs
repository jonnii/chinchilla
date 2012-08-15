namespace Chinchilla.Sample.StockTicker.Messages
{
    public class PriceMessage
    {
        public PriceMessage() { }

        public PriceMessage(string ticker, int price)
        {
            Ticker = ticker;
            Price = price;
        }

        public string Ticker { get; set; }

        public int Price { get; set; }
    }
}
