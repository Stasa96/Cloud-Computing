using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static int i = 0;
        static void Main(string[] args)
        {
            ChannelFactory<IInteraction> factory = new ChannelFactory<IInteraction>(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:11000/InputRequest"));

            IInteraction proxy = factory.CreateChannel();
            
           Console.WriteLine(   proxy.sendMsg("Raaaadi " + DateTime.Now));

            Console.ReadLine();
        }
    }
}
