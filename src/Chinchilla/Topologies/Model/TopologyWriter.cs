using System.IO;

namespace Chinchilla.Topologies.Model
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
            textWriter.WriteLine(exchange);
        }

        public void Visit(IBinding binding)
        {
            textWriter.WriteLine(binding);
        }
    }
}
