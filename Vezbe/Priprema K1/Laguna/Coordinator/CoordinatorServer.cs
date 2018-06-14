using Common;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Coordinator
{

    public class CoordinatorServer
    {
        ServiceHost sh;
        string endpointName = "InputCoordinator";
        RoleInstanceEndpoint endpoint;

        public CoordinatorServer()
        {
            endpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints[endpointName];

            sh = new ServiceHost(typeof(CoordinatorServerProvider));

            sh.AddServiceEndpoint(typeof(IClientInteraction), new NetTcpBinding(), $"net.tcp://{endpoint.IPEndpoint}/{endpointName}");

            Trace.WriteLine($"ServiceHost {endpointName} is created on {endpoint.IPEndpoint}");
        }

        public void Open()
        {
            try
            {
                sh.Open();
                Trace.WriteLine($"ServiceHost {endpointName} is opened on {endpoint.IPEndpoint}");
            }
            catch(Exception e)
            {
                Trace.WriteLine($"ServiceHost {endpointName} error {e.Message}");
            }
        }

        public void Close()
        {
            try
            {
                sh.Close();
                Trace.WriteLine($"ServiceHost {endpointName} is closed on {endpoint.IPEndpoint}");
            }
            catch (Exception e)
            {
                Trace.WriteLine($"ServiceHost {endpointName} error {e.Message}");
            }
        }
    }
}
