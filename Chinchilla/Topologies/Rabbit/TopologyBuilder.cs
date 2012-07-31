using System;
using RabbitMQ.Client;

namespace Chinchilla.Topologies.Rabbit
{
    public class TopologyBuilder : ITopologyVisitor
    {
        private readonly IModel model;

        public TopologyBuilder(IModel model)
        {
            this.model = Guard.NotNull(model, "model");
        }

        public void CreateExchange(string exchangeName, string exchangeType)
        {
            if (exchangeName == null)
            {
                throw new ArgumentNullException("exchangeName");
            }

            model.ExchangeDeclare(exchangeName, exchangeType, true);
        }

        public void Visit(IQueue queue)
        {
            throw new NotImplementedException();
        }

        public void Visit(IExchange exchange)
        {
            throw new NotImplementedException();
        }

        public void Visit(IBinding binding)
        {
            throw new NotImplementedException();
        }
    }
}