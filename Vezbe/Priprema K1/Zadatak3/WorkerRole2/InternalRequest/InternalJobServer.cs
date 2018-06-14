using Common;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WorkerRole2.InternalRequest
{
    public class InternalJobServer
    {
        ServiceHost sh;
        string name = "InternalRequest";
        RoleInstanceEndpoint endpoint;

        public InternalJobServer()
        {
            endpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints[name];

            sh = new ServiceHost(typeof(InternalJobServerProvider));

            sh.AddServiceEndpoint(typeof(IInteraction), new NetTcpBinding(), $"net.tcp://{endpoint.IPEndpoint}/{name}");
        }

        public void Open()
        {
            try
            {
                sh.Open();
                Trace.WriteLine($"ServiceHost {name} is opened on {endpoint.IPEndpoint}");
            }
            catch (Exception e)
            {
                Trace.WriteLine($"ServiceHost {name} open error {e.Message} on {endpoint.IPEndpoint}");
            }
        }

        public void Close()
        {
            try
            {
                sh.Close();
                Trace.WriteLine($"ServiceHost {name} is closed on {endpoint.IPEndpoint}");
            }
            catch (Exception e)
            {
                Trace.WriteLine($"ServiceHost {name} close error {e.Message} on {endpoint.IPEndpoint}");
            }
        }
    }
}
