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
            ChannelFactory<IPurchase> factory = new ChannelFactory<IPurchase>(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:8888/InputRequest"));
            IPurchase proxy = factory.CreateChannel();

            if (proxy.OrderItem("1", "1"))
            {
                Console.WriteLine("Uspesno");
            }
            else
            {
                Console.WriteLine("Neuspesno");
            }

            if (proxy.OrderItem("2", "2"))
            {
                Console.WriteLine("Uspesno");
            }
            else
            {
                Console.WriteLine("Neuspesno");
            }

            if (proxy.OrderItem("2", "2"))
            {
                Console.WriteLine("Uspesno");
            }
            else
            {
                Console.WriteLine("Neuspesno");
            }

            Console.ReadKey();
            //proxy.OrderItem("2", "2");
            //proxy.OrderItem("2", "2");
            ////proxy.OrderItem("3", "3");
            ////proxy.OrderItem("4", "4");
            ////proxy.OrderItem("5", "5");
        }
    }
}
