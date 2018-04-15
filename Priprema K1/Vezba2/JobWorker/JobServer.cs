using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WCFContracts;

namespace JobWorker
{
    public class JobServer
    {
        ServiceHost sh;
        string externalEndpointName = "InputRequest";
        public JobServer()
        {
            RoleInstanceEndpoint inputEndPoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints[externalEndpointName];
            string endpoint = $"net.tcp://{inputEndPoint.IPEndpoint}/{externalEndpointName}";

            sh = new ServiceHost(typeof(JobServerProvider));

            sh.AddServiceEndpoint(typeof(IJob), new NetTcpBinding(), endpoint);
        }

        public void Open()
        {
            try
            {
                sh.Open();
                Trace.WriteLine($"ServiceHost opened on {externalEndpointName}");
            }
            catch (Exception)
            {
                Trace.WriteLine("ServiceHost error");
            }

        }

        public void Close()
        {
            try
            {
                sh.Close();
                Trace.WriteLine("ServiceHost closed on " + externalEndpointName);
            }
            catch (Exception)
            {
                Trace.WriteLine("ServiceHost error");
            }
        }
    }
}
