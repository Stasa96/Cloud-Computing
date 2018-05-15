using AzureService_data;
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
            

            for (int i = 0; i < 10; i++)
            {
                ChannelFactory<IRequest> factory = new ChannelFactory<IRequest>(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:11001/InputRequest"));
                IRequest proxy = factory.CreateChannel();
                RequestCountInfoWCF r = proxy.Request();

                Console.WriteLine(r.Instance + " " + r.RequestCnt);
            }
            Console.ReadKey();
        }
    }
}
