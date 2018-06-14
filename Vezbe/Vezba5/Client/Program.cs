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
            ChannelFactory<IPurchase> bankFactory = new ChannelFactory<IPurchase>(new NetTcpBinding(), new EndpointAddress($"net.tcp://127.255.0.2:11000/InputRequest"));
            IPurchase proxy  = bankFactory.CreateChannel();


            proxy.OrderItem("001", "001");
            Console.WriteLine(proxy.ListBooks());
            Console.WriteLine(proxy.ListUsers());
            Console.WriteLine("============================================");

            proxy.OrderItem("002", "002");
            Console.WriteLine(proxy.ListBooks());
            Console.WriteLine(proxy.ListUsers());
            Console.WriteLine("============================================");

            proxy.OrderItem("003", "003");
            Console.WriteLine(proxy.ListBooks());
            Console.WriteLine(proxy.ListUsers());
            Console.WriteLine("============================================");


            Console.ReadKey();
        }
    }
}
