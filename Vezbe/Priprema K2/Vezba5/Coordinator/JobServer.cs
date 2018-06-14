
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
    public class JobServer
    {
        private ServiceHost serviceHost;
        private string externalEndpointName = "InputRequest";
        RoleInstanceEndpoint instanceEndpoint;
       

        public JobServer()
        {
            NetTcpBinding binding = new NetTcpBinding();

            instanceEndpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints[externalEndpointName];
            string endpoint = String.Format($"net.tcp://{instanceEndpoint.IPEndpoint}/{externalEndpointName}");

            serviceHost = new ServiceHost(typeof(JobServerProvider));
            serviceHost.AddServiceEndpoint(typeof(IPurchase), binding, endpoint);
    

           }

        public void Open()
        {
            try
            {
                serviceHost.Open();
                Trace.WriteLine($"Service host for {externalEndpointName} endpoint type opened successfully at {DateTime.Now} {instanceEndpoint.IPEndpoint}");
            }
            catch (Exception e)
            {
                Trace.WriteLine($"Service host open error for {externalEndpointName} endpoint type. Error message is: {e.Message}");
            }
        }

        public void Close()
        {
            try
            {
                serviceHost.Close();
                Trace.WriteLine($"Service host for {externalEndpointName} endpoint type closed successfully at {DateTime.Now}");
            }
            catch (Exception e)
            {
                Trace.WriteLine($"Service host close error for {externalEndpointName} endpoint type. Error message is: {e.Message}");
            }
        }
    }
}
