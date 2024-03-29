﻿using Common;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace JobWorker2
{
    public class JobServerInternal
    {
        ServiceHost sh;
        string name = "RequestInternal";
        RoleInstanceEndpoint endpoint;
        public JobServerInternal()
        {
            endpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints[name];

            sh = new ServiceHost(typeof(JobServerProviderInternal));

            sh.AddServiceEndpoint(typeof(IContract), new NetTcpBinding(), $"net.tcp://{endpoint.IPEndpoint}/{name}");
        }

        public void Open()
        {
            try
            {
                sh.Open();
                Trace.WriteLine($"ServiceHost {name} is opened on {endpoint.IPEndpoint}");
            }
            catch (Exception e)
            {
                Trace.WriteLine($"ServiceHost {name} error {e.Message}");
            }
        }

        public void Close()
        {
            try
            {
                sh.Close();
                Trace.WriteLine($"ServiceHost {name} is closed on {endpoint.IPEndpoint}");
            }
            catch (Exception e)
            {
                Trace.WriteLine($"ServiceHost {name} error {e.Message}");
            }
        }
    }
}
