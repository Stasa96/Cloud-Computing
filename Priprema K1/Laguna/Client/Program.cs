using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            ChannelFactory<IClientInteraction> factory = new ChannelFactory<IClientInteraction>(new NetTcpBinding(), new EndpointAddress($"net.tcp://localhost:11000/InputCoordinator"));

            IClientInteraction proxy = factory.CreateChannel();

            if (proxy.Registration("stefanrrr", "stefan", "Ruvceski", "terminator", "11223344"))
            {
                Console.WriteLine("Registration success");
            }
            else
            {
                Console.WriteLine("Registration error");
            }

            proxy.AddBook("Na drini cuprija", 6, 850);
            proxy.AddBook("Sismis", 4, 750);
            proxy.AddBook("Elon Musk", 9, 1100);
            proxy.AddBook("Bubasvaba", 9, 650);

            
            if(proxy.OrderBook("Na drini cuprija", "stefanrrr", "terminator", 3))
            {
                Console.WriteLine("Book is ordered");
            }
            else
            {
                Console.WriteLine("Error order");
            }

            if (proxy.ReturBook("Na drini cuprija", "stefanrrr", "terminator", 2))
            {
                Console.WriteLine("Book is returned");
            }
            else
            {
                Console.WriteLine("Error returning");
            }

            Console.ReadKey();
        }
    }
}
