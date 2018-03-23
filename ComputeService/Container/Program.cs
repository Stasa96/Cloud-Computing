using ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Container
{
    class Program
    {
        static ServiceHost svc;
        static void Main(string[] args)
        {
                OpenServiceHost(args[0]);
                while (Container.flag == -1) { }
                CloseServiceHost();
                Container.flag = -1;
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
