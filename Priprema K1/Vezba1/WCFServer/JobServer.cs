using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WCFContracts;

namespace WCFServer
{
    public class JobServer
    {
        ServiceHost sh;
        public JobServer()
        {
            Start();
        }

        public void Start()
        {
            sh = new ServiceHost(typeof(HealthMonitoring));

            NetTcpBinding binding = new NetTcpBinding();
            sh.AddServiceEndpoint(typeof(IHealthMonitoring), binding, new Uri("net.tcp://localhost:11000/HealthMonitoring"));

            sh.Open();
            Console.WriteLine($"Server is ready on {IPAddress.Loopback}:6000");
        }

        public void Stop()
        {
            sh.Close();
            Console.WriteLine("Server stopped");
        }
    }
}
