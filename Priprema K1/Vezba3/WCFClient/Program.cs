using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WCFContracts;

namespace WCFClient
{
    class Program
    {
         static IJob proxy;
        static void Main(string[] args)
        {
            Connect();
            Console.WriteLine(proxy.doCalculus(5));
            Console.ReadKey();
        }

        private static void Connect()
        {
            ChannelFactory<IJob> factory = new ChannelFactory<IJob>( new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:11000/InputRequest"));
            proxy = factory.CreateChannel();
            
        }
    }
}
