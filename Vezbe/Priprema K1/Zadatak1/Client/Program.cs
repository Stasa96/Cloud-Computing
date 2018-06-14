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
        static void Main(string[] args)
        {
            ChannelFactory<IContract> factory = new ChannelFactory<IContract>(new NetTcpBinding(),new EndpointAddress("net.tcp://localhost:11000/InputRequest"));
            IContract contract = factory.CreateChannel();

            int i = 1;

            while (true)
            {
                try
                {
                    if (i% 3 != 0)
                    {
                        Console.WriteLine(contract.SendMsg("Message " + i));
                        Thread.Sleep(2000);
                    }
                    else
                    {
                        
                        contract.SendMsg(null);
                        
                    }
                }
                catch (FaultException<MyException> ex)
                {
                    Console.WriteLine("Error: " + ex.Detail.Reason);
                }

                i++;
            }
        }
    }
}
