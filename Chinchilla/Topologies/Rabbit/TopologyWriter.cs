using System.IO;

namespace Chinchilla.Topologies.Rabbit
{
    public class TopologyWriter : ITopologyVisitor
    {
        private readonly TextWriter textWriter;

        public TopologyWriter(TextWriter textWriter)
        {
            this.textWriter = textWriter;
        }

        public void Visit(IQueue queue)
        {
            textWriter.WriteLine(queue);
        }

        public void Visit(IExchange exchange)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(IBinding binding)
        {
            throw new System.NotImplementedException();
        }
    }
}
