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
    public class JobServerProviderInternal : IInteraction
    {
     

       
        List<IInteraction> list;
        List<Task<string>> tasks = new List<Task<string>>();
        public string sendMsg(string msg)
        {

            string s3 = "WorkerRole2";
            string s4 = "RequestInternal";
            int cnt = RoleEnvironment.Roles["WorkerRole2"].Instances.Count;

            Trace.WriteLine($"Message: {msg}");

            list = new List<IInteraction>();
            for (int i = 0; i < cnt; i++)
            {
                RoleInstanceEndpoint endpoint = RoleEnvironment.Roles[s3].Instances[i].InstanceEndpoints[s4];
                ChannelFactory<IInteraction> factory = new ChannelFactory<IInteraction>(new NetTcpBinding(), new EndpointAddress($"net.tcp://{endpoint.IPEndpoint}/{s4}"));
                list.Add(factory.CreateChannel());
            }

            foreach (IInteraction i in list)
            {
                tasks.Add(Task.Factory.StartNew(() => i.sendMsg(msg)));
            }

            return "ok";
        }
    }
}
