using ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Container
{
    class Program
    {
        static ServiceHost svc;
        public static int flag = -1;
        public static string port;
        
        static void Main(string[] args)
        {
            OpenServiceHost(args[0]);
            port = args[0];
            Console.ReadKey();
            
        }

        public static void OpenServiceHost(string port)
        {
            svc = new ServiceHost(typeof(Container));
            svc.AddServiceEndpoint(typeof(IContainer),
            new NetTcpBinding(),
            new Uri($"net.tcp://localhost:{port}/IContainer"));

            svc.Open();
            Console.WriteLine("Service host is open on " + port + " port.\n_______________________________________________");
        }

        public static void CloseServiceHost()
        {
            svc.Close();
        }
    }
}
