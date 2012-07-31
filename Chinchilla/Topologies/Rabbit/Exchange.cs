namespace Chinchilla.Topologies.Rabbit
{
    public class Exchange : Bindable, IExchange
    {
        public Exchange(string name, ExchangeType exchangeType)
        {
            Name = name;
            ExchangeType = exchangeType;
        }

        public string Name { get; set; }

        public ExchangeType ExchangeType { get; set; }

        public Durability Durability { get; set; }

        public bool IsAutoDelete { get; set; }

        public bool IsInternal { get; set; }

        public string AlternateExchange { get; set; }

        public bool HasAlternateExchange
        {
            get { return !string.IsNullOrEmpty(AlternateExchange); }
        }

        public override void Visit(ITopologyVisitor visitor)
        {
            visitor.Visit(this);

            foreach (var binding in Bindings)
            {
                binding.Visit(visitor);
            }
        }

        public override string ToString()
        {
            return string.Format("[Exchange Name: {0}]", Name);
        }
    }
}