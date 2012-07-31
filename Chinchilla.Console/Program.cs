using System;
using Chinchilla.Topologies.Rabbit;

namespace Chinchilla.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var bus = Depot.Connect("localhost"))
            {
                var handler = new Action<HelloWorldMessage>(System.Console.WriteLine);

                using (bus.Subscribe(handler))
                {
                    var topology = bus.Topology;

                    topology.Visit(new TopologyWriter(System.Console.Out));

                    System.Console.WriteLine("Waiting for you!");
                    System.Console.ReadLine();
                }
            }
        }
    }

    public class HelloWorldMessage { }
}
