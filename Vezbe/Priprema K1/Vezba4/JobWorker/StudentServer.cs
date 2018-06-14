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
    
    public class StudentServer
    {
        ServiceHost sh;
        string roleName = "InputRequest";
        RoleInstanceEndpoint endpoint;

        public StudentServer()
        {
            endpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints[roleName];
            sh = new ServiceHost(typeof(StudentServerProvider));

            sh.AddServiceEndpoint(typeof(IStudent), new NetTcpBinding(), $"net.tcp://{endpoint.IPEndpoint}/{roleName}");
        }

        public void Open()
        {
            try
            {
                sh.Open();
                Trace.WriteLine($"Servicehost {roleName} is opened on {endpoint.IPEndpoint}");
            }
            catch(Exception e)
            {
                Trace.WriteLine($"ServiceHost error {e.Message}");
            }
        }

        public void Close()
        {
            try
            {
                sh.Close();
                Trace.WriteLine($"Servicehost {roleName} is closed on {endpoint.IPEndpoint}");
            }
            catch (Exception e)
            {
                Trace.WriteLine($"ServiceHost error {e.Message}");
            }
        }
    }
}
