using Common;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore
{
    public class BookstoreServer
    {
        ServiceHost sh;
        RoleInstanceEndpoint endpoint;
        string internalEndpoint = "InternalBookstore";
        public BookstoreServer()
        {
            endpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints[internalEndpoint];

            sh = new ServiceHost(typeof(BookstoreServerProvider));

            sh.AddServiceEndpoint(typeof(IBookstore), new NetTcpBinding(), $"net.tcp://{endpoint.IPEndpoint}/{internalEndpoint}");

        }

        public void Open()
        {
            try
            {
                sh.Open();
                Trace.WriteLine($"ServiceHost for {internalEndpoint} is opened on {endpoint.IPEndpoint}");
            }
            catch(Exception e)
            {
                Trace.WriteLine($"ServiceHost Error {e.Message}");
            }
        }

        public void Close()
        {
            try
            {
                sh.Close();
                Trace.WriteLine($"ServiceHost for {internalEndpoint} is closed on {endpoint.IPEndpoint}");
            }
            catch (Exception e)
            {
                Trace.WriteLine($"ServiceHost Error {e.Message}");
            }
        }
    }
}
