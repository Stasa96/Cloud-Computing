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
            ChannelFactory<IPurchase> bankFactory = new ChannelFactory<IPurchase>(new NetTcpBinding(), new EndpointAddress($"net.tcp://localhost:11000/InputRequest"));
            IPurchase proxy  = bankFactory.CreateChannel();


            Console.WriteLine( proxy.ListBooks());

            Console.ReadKey();
        }
    }
}
