using System;
using System.Collections.Generic;
using Chinchilla.Logging;
using RabbitMQ.Client;

namespace Chinchilla
{
    public class ModelFactory : IModelFactory
    {
        private readonly ILogger logger = Logger.Create<ModelFactory>();

        private readonly List<IModelReference> references = new List<IModelReference>();

        private IConnection connection;

        public ModelFactory(IConnection connection)
        {
            this.connection = connection;
        }

        public bool IsOpen
        {
            get { return connection.IsOpen; }
        }

        public int NumReferences
        {
            get { return references.Count; }
        }

        public IModelReference CreateModel()
        {
            var reference = new ModelReference(connection.CreateModel());
            reference.Disposed += Untrack;

            references.Add(reference);
            return reference;
        }

        public void Reconnect(IConnection newConnection)
        {
            logger.Debug("Resetting model factory connection");

            connection = newConnection;

            foreach (var reference in references)
            {
                reference.Reconnect(connection.CreateModel());
            }
        }

        public bool IsTracking(IModelReference reference)
        {
            return references.Contains(reference);
        }

        public void Untrack(IModelReference modelReference)
        {
            references.Remove(modelReference);
        }

        private void Untrack(object sender, EventArgs eventArgs)
        {
            var reference = (IModelReference)sender;

            reference.Disposed -= Untrack;
            Untrack(reference);
        }
    }
}