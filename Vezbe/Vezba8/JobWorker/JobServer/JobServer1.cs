using AzureService_data;
using Common;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace JobWorker.JobServer
{
    public class JobServer1
    {
        private ServiceHost serviceHost;
        RoleInstanceEndpoint inputEndPoint;
         
        private String externalEndpointName = "InputRequest";
        public JobServer1()
        {
            inputEndPoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints[externalEndpointName];
            JobServerProvider.instanceId = inputEndPoint.RoleInstance.Id.Split('_')[2];
            string endpoint = String.Format("net.tcp://{0}/{1}",
           inputEndPoint.IPEndpoint, externalEndpointName);
            serviceHost = new ServiceHost(typeof(JobServerProvider));
            NetTcpBinding binding = new NetTcpBinding();
            serviceHost.AddServiceEndpoint(typeof(IRequest), binding, endpoint);

            DataRepository initial = new DataRepository();
            RequestCountInfo r = new RequestCountInfo(JobServerProvider.instanceId);
            JobServerProvider.repository.AddRequest(r);
        }
        public void Open()
        {
            try
            {
                serviceHost.Open();
                Trace.TraceInformation(String.Format("{0} Host for {1} endpoint type opened successfully at {2} {3}", JobServerProvider.instanceId, externalEndpointName, DateTime.Now,inputEndPoint.IPEndpoint));
                
            }
            catch (Exception e)
            {
                Trace.TraceInformation("Host open error for {0} endpoint type. Error message is: {1}. ", externalEndpointName, e.Message);
            }
        }
        public void Close()
        {
            try
            {
                serviceHost.Close();
                Trace.TraceInformation(String.Format("Host for {0} endpoint type closed successfully at {1}", externalEndpointName, DateTime.Now));
            }
            catch (Exception e)
            {
                Trace.TraceInformation("Host close error for {0} endpoint type. Error message is: {1}. ", externalEndpointName, e.Message);
            }
        }
    }
}
