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
        string externalEndpoint = "InputRequest";

        public JobServer()
        {
            RoleInstanceEndpoint endpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints[externalEndpoint];
            string endpointname = $"net.tcp://{endpoint.IPEndpoint}/{externalEndpoint}";

            sh = new ServiceHost(typeof(JobServerProvider));

            sh.AddServiceEndpoint(typeof(IJob), new NetTcpBinding(), endpointname);
        }

        public void Open()
        {
            try
            {
                sh.Open();
                Trace.WriteLine("ServiceHost open on " + externalEndpoint + " opened.");
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
                Trace.WriteLine("ServiceHost on " + externalEndpoint + " closed.");

            }
            catch (Exception)
            {
                Trace.WriteLine("ServiceHost error");
            }
        }

    }
}
