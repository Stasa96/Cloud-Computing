using Common;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WorkerRole1
{
    public class JobServer
    {
        ServiceHost sh;
        string name = "InputRequest";
        RoleInstanceEndpoint endpoint;

        public JobServer()
        {
            endpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints[name];

            sh = new ServiceHost(typeof(JobServerProvider));

            sh.AddServiceEndpoint(typeof(IInteraction), new NetTcpBinding(), $"net.tcp://{endpoint.IPEndpoint}/{name}");
        }

        public void Open()
        {
            try
            {
                sh.Open();
                Trace.WriteLine($"SH open");
            }catch(Exception e)
            {
                Trace.WriteLine($"SH error {e.Message}");
            }
        }

        public void Close()
        {
            try
            {
                sh.Close();
                Trace.WriteLine($"SH close");
            }
            catch (Exception e)
            {
                Trace.WriteLine($"SH error {e.Message}");
            }
        }
    }
}
