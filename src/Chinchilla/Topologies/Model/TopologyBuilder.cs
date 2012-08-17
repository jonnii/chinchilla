using System;
using System.Collections.Generic;
using System.Linq;

namespace Chinchilla.Topologies.Model
{
    public class TopologyBuilder : ITopologyVisitor
    {
        private readonly IModelReference model;

        private readonly List<object> visited = new List<object>();

        public TopologyBuilder(IModelReference model)
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

                    model.Execute(m =>
                        m.QueueDeclare(
                            queue.Name,
                            queue.Durability == Durability.Durable,   // durable
                            queue.IsExclusive,  // exclusive
                            queue.IsAutoDelete,  // auto-delete
                            args));
                }
                else
                {
                    var declared = model.Execute(m => m.QueueDeclare());
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

                model.Execute(
                    m => m.ExchangeDeclare(
                        exchangeName,
                        exchangeType.ToString().ToLower(),
                        exchange.Durability == Durability.Durable,
                        exchange.IsAutoDelete,
                        new Dictionary<string, object>()));
            });
        }

        public void Visit(IBinding binding)
        {
            VisitOnce(binding, () =>
            {
                model.Execute(m =>
                {
                    Action<string, string, string> bindFunction;
                    if (binding.Bindable is IQueue)
                    {
                        bindFunction = m.QueueBind;
                    }
                    else
                    {
                        // Reverse from/to for binding exchanges, because the parameters of ExchangeBind
                        // don't match QueueBind
                        bindFunction = (from, to, keys) => m.ExchangeBind(to, from, keys);
                    }

                    if (binding.RoutingKeys.Any())
                    {
                        foreach (var key in binding.RoutingKeys)
                        {
                            bindFunction(binding.Bindable.Name, binding.Exchange.Name, key);
                        }
                    }
                    else
                    {
                        bindFunction(binding.Bindable.Name, binding.Exchange.Name, "#");
                    }
                });

                visited.Add(binding);
            });
        }
    }
}