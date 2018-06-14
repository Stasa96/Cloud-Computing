using Common;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace TransactionCoordinator
{
    public class CoordinatorServer
    {
        ServiceHost sh;
        string roleName = "InputRequest";
        RoleInstanceEndpoint endpoint;

        public CoordinatorServer()
        {
            endpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints[roleName];

            sh = new ServiceHost(typeof(CoordinatorServerProvider));

            sh.AddServiceEndpoint(typeof(IPurchase), new NetTcpBinding(), $"net.tcp://{endpoint.IPEndpoint}/{roleName}");
        }

        public void Open()
        {
            try
            {
                sh.Open();
                Trace.WriteLine($"ServiceHost {roleName} is opened on {endpoint.IPEndpoint}");
            }
            catch (Exception e)
            {
                Trace.WriteLine($"ServiceHost error {e.Message}");
            }
        }

        public void Close()
        {
            try
            {
                sh.Close();
                Trace.WriteLine($"ServiceHost {roleName} is closed on {endpoint.IPEndpoint}");
            }
            catch (Exception e)
            {
                Trace.WriteLine($"ServiceHost error {e.Message}");
            }
        }
    }
}
