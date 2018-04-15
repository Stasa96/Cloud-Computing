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

            Meni();
            Console.ReadKey();
        }

        private static void Connect()
        {
            ChannelFactory<IJob> factory = new ChannelFactory<IJob>(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:11000/InputRequest"));

            proxy = factory.CreateChannel();
        }

        private static void Meni()
        {
            int flag = -1;
            while (flag != 0)
            {
                Console.Clear();
                Console.WriteLine("_________________________________");
                Console.WriteLine("1.DoCalculus");
                Console.WriteLine("0.Exit");

                try
                {
                    flag = int.Parse(Console.ReadLine());
                    if (flag == 1)
                    {
                        Console.WriteLine("_________________________________");
                        Console.Write("Enter number: ");
                        Console.WriteLine("Sum is: " + proxy.DoCalculus(int.Parse(Console.ReadLine())));
                        Console.WriteLine("Press any key to continue.");
                        Console.ReadKey();
                    }
                    else if(flag == 0)
                    {
                        break;
                    }
                    else
                    {
                        flag = -1;
                    }
                }
                catch (Exception)
                {
                    flag = -1;
                }
            }

            Console.WriteLine("Client stoped.");
        }
    }
}
