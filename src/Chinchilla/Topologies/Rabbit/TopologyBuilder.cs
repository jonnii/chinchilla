using System;
using System.Collections.Generic;
using RabbitMQ.Client;

namespace Chinchilla.Topologies.Rabbit
{
    public class TopologyBuilder : ITopologyVisitor
    {
        private readonly IModel model;

        private readonly List<object> visited = new List<object>();

        public TopologyBuilder(IModel model)
        {
            this.model = Guard.NotNull(model, "model");
        }

        private void VisitOnce<T>(T t, Action act)
        {
            if (visited.Contains(t))
            {
                return;
            }

            act();

            visited.Add(t);
        }

        public void Visit(IQueue queue)
        {
            VisitOnce(queue, () =>
            {
                if (queue.HasName)
                {
                    var args = new Dictionary<string, object>();

                    if (queue.QueueAutoExpire.HasValue)
                    {
                        args.Add("x-expires", queue.QueueAutoExpire.Value.Milliseconds);
                    }

                    model.QueueDeclare(
                        queue.Name,
                        true,   // durable
                        false,  // exclusive
                        false,  // auto-delete
                        args);
                }
                else
                {
                    var declared = model.QueueDeclare();
                    queue.Name = declared.QueueName;
                }
            });
        }

        public void Visit(IExchange exchange)
        {
            VisitOnce(exchange, () =>
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
            });
        }

        public void Visit(IBinding binding)
        {
            VisitOnce(binding, () =>
            {
                model.QueueBind(
                    binding.Bindable.Name,
                    binding.Exchange.Name, "#");

                visited.Add(binding);
            });
        }
    }
}