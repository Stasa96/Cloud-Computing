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
            

            ChannelFactory<IPurchase> factory = new ChannelFactory<IPurchase>(new NetTcpBinding(),new EndpointAddress($"net.tcp://localhost:11000/InputRequest"));

            IPurchase proxy = factory.CreateChannel();


            if (proxy.orderItem("123", "123"))
            {
                Console.WriteLine("Ordered book successfuly");
            }
            else
            {
                Console.WriteLine("Error");
            }
            Console.ReadKey();
        }
    }
}
