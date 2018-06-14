using InterroleContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace JobInvoker
{
    class Program
    {
        private static IJob proxy;

        static void Main(string[] args)
        {
            int rez = -1 ;
            do
            {
                Console.Write("Unesite gornju granicu intervala: ");
                int gornjaGranica = Int32.Parse(Console.ReadLine());

                Connect();                                          // povezujemo ga sa serverom
                rez = proxy.DoCalculus(gornjaGranica);          // pozivamo metodu servera

                Console.WriteLine($"Suma prvih {gornjaGranica} prirodnih brojeva je: {rez}");
            } while (rez != 0);
            Console.ReadKey();
        }

        public static void Connect()
        {
            var binding = new NetTcpBinding();
            ChannelFactory<IJob> factory = new ChannelFactory<IJob>(binding, new EndpointAddress("net.tcp://localhost:6000/InputRequest"));     // ovde mora biti InputRequest!
            proxy = factory.CreateChannel();
        }
    }
}
