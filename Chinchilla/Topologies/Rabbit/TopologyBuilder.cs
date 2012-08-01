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

        public void Visit(IQueue queue)
        {
            if (queue.HasName)
            {
                model.QueueDeclare(
                    queue.Name,
                    true,   // durable
                    false,  // exclusive
                    false,  // auto-delete
                    null);
            }
            else
            {
                var declared = model.QueueDeclare();
                queue.Name = declared.QueueName;
            }
        }

        public void Visit(IExchange exchange)
        {
            var exchangeName = exchange.Name;
            var exchangeType = exchange.Type;

            if (string.IsNullOrEmpty(exchangeName))
            {
                throw new ArgumentException("exchange needs a name");
            }

            model.ExchangeDeclare(
                exchangeName,
                exchangeType.ToString().ToLower(),
                true);  // durable
        }

        public void Visit(IBinding binding)
        {
            model.QueueBind(
                binding.Bindable.Name,
                binding.Exchange.Name, "#");
        }
    }
}