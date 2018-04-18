using Common;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace JobWorker1
{
    public class InternalJobServerProvider : IContract
    {
        List<IContract> proxy = new List<IContract>();
        List<Task<string>> tasks = new List<Task<string>>();
        public string SendMsg(string msg)
        {
            if(msg != null)
            {
                SendInternal(msg);

                Trace.WriteLine("Message: " + msg);

                return "Message is received";
            }
            proxy.Clear();
            tasks.Clear();
            return null;
        }

        private void SendInternal(string msg)
        {
            string name = "RequestInternal";
            int cnt = RoleEnvironment.Roles["JobWorker2"].Instances.Count;

            for (int i = 0; i < cnt; i++)
            {
                RoleInstanceEndpoint endpoint = RoleEnvironment.Roles["JobWorker2"].Instances[i].InstanceEndpoints[name];
                ChannelFactory<IContract> factory = new ChannelFactory<IContract>(new   NetTcpBinding(),new EndpointAddress($"net.tcp://{endpoint.IPEndpoint}/{name}"));
                proxy.Add(factory.CreateChannel());
            }

            foreach (IContract c in proxy)
            {
                tasks.Add(Task<string>.Factory.StartNew(() => c.SendMsg(msg)));
            }

            Task.WaitAll(tasks.ToArray());

            foreach (Task<string> t in tasks)
            {
                Trace.WriteLine($"From task {t.Id} returned message is {t.Result}");
            }
        }
    }
}