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
    public class JobServerProvider : IInteraction
    {

        IInteraction brother = null;
        List<IInteraction> list = null;
        List<Task<string>> tasks = new List<Task<string>>();

        public string sendMsg(string msg)
        {
            Connect(msg);

            

           



            return "ok";
        }

        private void Connect(string msg)
        {
            string s = "InputRequest";

            string s1 = "WorkerRole1";
            string s2 = "InternalRequest";

            string s3 = "WorkerRole2";
            string s4 = "RequestInternal";
            int cnt = RoleEnvironment.Roles["WorkerRole2"].Instances.Count;

            if ((RoleEnvironment.CurrentRoleInstance.Id.Split('_'))[2] == "0")
            {
                Trace.WriteLine("Usao sam u instancu 1 ojsa");
                RoleInstanceEndpoint endpoint = RoleEnvironment.Roles[s1].Instances[1].InstanceEndpoints[s2];
                ChannelFactory<IInteraction> factory = new ChannelFactory<IInteraction>(new NetTcpBinding(),new EndpointAddress($"net.tcp://{endpoint.IPEndpoint}/{s2}"));
                brother = factory.CreateChannel();
                brother.sendMsg(msg);
            }
            else if ((RoleEnvironment.CurrentRoleInstance.Id.Split('_'))[2] == "1")
            {
                Trace.WriteLine("Usao sam u instancu 2 ojsa " + msg);
                list = new List<IInteraction>();
                
                    RoleInstanceEndpoint endpoint = RoleEnvironment.Roles[s3].Instances[1].InstanceEndpoints[s4];
                    ChannelFactory<IInteraction> factory = new ChannelFactory<IInteraction>(new NetTcpBinding(), new EndpointAddress($"net.tcp://{endpoint.IPEndpoint}/{s4}"));
                    list.Add(factory.CreateChannel());

                    foreach (IInteraction ii in list)
                    {
                        tasks.Add(Task.Factory.StartNew(() => ii.sendMsg(msg)));
                    }

                    Task.WaitAll(tasks.ToArray());
                
            }
        }
    }
}
