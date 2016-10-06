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

        public bool IsOpen => connection.IsOpen;

        public IModelReference CreateModel()
        {
            var reference = new ModelReference(connection.CreateModel());
            Track(reference);
            return reference;
        }

        public IModelReference CreateModel(string tag)
        {
            var reference = new ModelReference(connection.CreateModel(), tag);
            Track(reference);
            return reference;
        }

        public override void Dispose()
        {
            logger.Debug("Disposing model factory");
            connection.Dispose();
            base.Dispose();
        }
    }
}