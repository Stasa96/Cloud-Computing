using Common;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WorkerRole
{
    public class InputJobServer
    {
        private ServiceHost serviceHost;

        private String externalEndpointName = "InputRequest";
        public InputJobServer()
        {
            serviceHost = new ServiceHost(typeof(InputJobServerProvider));

            NetTcpBinding binding = new NetTcpBinding();
            //Default timeout je 60 sekundi pa puca na proxy tokom debug-a
            binding.OpenTimeout = new TimeSpan(0, 10, 0);
            binding.CloseTimeout = new TimeSpan(0, 10, 0);
            binding.SendTimeout = new TimeSpan(0, 10, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 10, 0);

            RoleInstanceEndpoint internalEndpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints[externalEndpointName];
            string endpointAddress = String.Format("net.tcp://{0}/{1}", internalEndpoint.IPEndpoint, externalEndpointName);

            try
            {
                serviceHost.AddServiceEndpoint(typeof(IFindFilm), binding, endpointAddress);
                Trace.TraceInformation("Host for {0} endpoint type created.", externalEndpointName);
            }
            catch (Exception e)
            {

                Trace.TraceError("ERROR: {0}", e.Message);
            }
        }
        public void Open()
        {
            try
            {
                serviceHost.Open();
                Trace.TraceInformation(String.Format("Host for {0} endpoint type opened successfully at {1} ", externalEndpointName, DateTime.Now));
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
