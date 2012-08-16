namespace Chinchilla.Topologies.Model
{
    public class Exchange : Bindable, IExchange
    {
        public Exchange(string name, ExchangeType exchangeType)
        {
            Name = name;
            Type = exchangeType;
        }

        public ExchangeType Type { get; set; }

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