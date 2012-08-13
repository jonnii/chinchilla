using RabbitMQ.Client;

namespace Chinchilla
{
    public class ModelFactory : IModelFactory
    {
        private readonly IConnection connection;

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
            return new ModelReference(connection.CreateModel());
        }
    }
}