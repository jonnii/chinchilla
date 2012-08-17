using Chinchilla.Logging;
using RabbitMQ.Client;

namespace Chinchilla
{
    public class ModelFactory : TrackableFactory<ModelReference>, IModelFactory
    {
        private readonly ILogger logger = Logger.Create<ModelFactory>();

        private IConnection connection;

        public ModelFactory(IConnection connection)
        {
            this.connection = connection;
        }

        public bool IsOpen
        {
            get { return connection.IsOpen; }
        }

        public IModelReference CreateModel()
        {
            var reference = new ModelReference(connection.CreateModel());
            Track(reference);
            return reference;
        }

        public void Reconnect(IConnection newConnection)
        {
            logger.Debug("Resetting model factory connection");

            connection = newConnection;

            foreach (var reference in Tracked)
            {
                reference.Reconnect(connection.CreateModel());
            }
        }

        public override void Dispose()
        {
            logger.Debug("Disposing model factory");
            connection.Dispose();
            base.Dispose();
        }
    }
}