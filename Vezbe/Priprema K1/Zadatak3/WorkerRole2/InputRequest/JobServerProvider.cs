using Common;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace WorkerRole2.InputRequest
{
    public class JobServerProvider : IInteraction
    {
        List<IInteraction> proxy = new List<IInteraction>();
        List<Task<bool>> tasks = new List<Task<bool>>();
        IInteraction1 p;
        public bool SendCnt(int cnt)
        {
            if (Send(cnt) == 1)
            {
                foreach(IInteraction i in proxy)
                {
                    tasks.Add(Task<bool>.Factory.StartNew(() => i.SendCnt(cnt)));
                }
                return true;
            }
            else if (Send(cnt) == 2)
            {
                p.SendAgain("ponovi transakciju", cnt);
                return true;
            }
            else
            {
                try
                {
                    Trace.WriteLine($"Message: {cnt}");
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            
        }

        private int Send(int c)
        {
            proxy.Clear();
            string role = "WorkerRole2";
            string name = "InternalRequest";

            int j = 0;

            int cnt = RoleEnvironment.Roles[role].Instances.Count;
            if(((RoleEnvironment.CurrentRoleInstance.Id.Split('_'))[2]) == "0")
            {
                Trace.WriteLine($"Usao u prvu broj je {c}\n");
                for (int i = 0; i < cnt; i++)
                {
                    
                    if(RoleEnvironment.CurrentRoleInstance.InstanceEndpoints[name].IPEndpoint != RoleEnvironment.Roles[role].Instances[i].InstanceEndpoints[name].IPEndpoint)
                    {
                        RoleInstanceEndpoint endpont = RoleEnvironment.Roles[role].Instances[i].InstanceEndpoints[name];
                        ChannelFactory<IInteraction> factory = new ChannelFactory<IInteraction>(new NetTcpBinding(), new EndpointAddress($"net.tcp://{endpont.IPEndpoint}/{name}"));
                        proxy.Add(factory.CreateChannel());
                    }
                }
                return 1;
            }
            else if((RoleEnvironment.CurrentRoleInstance.Id.Split('_'))[2] == "1")
            {
                Trace.WriteLine($"Usao u drugu broj je {c}\n");
                role = "WorkerRole1";

                RoleInstanceEndpoint endpoint = RoleEnvironment.Roles[role].Instances[0].InstanceEndpoints[name];

                ChannelFactory<IInteraction1> factory = new ChannelFactory<IInteraction1>(new NetTcpBinding(), new EndpointAddress($"net.tcp://{endpoint.IPEndpoint}/{name}"));
                p = factory.CreateChannel();

                return 2;
            }
            else if((RoleEnvironment.CurrentRoleInstance.Id.Split('_'))[2] != "0" && (RoleEnvironment.CurrentRoleInstance.Id.Split('_'))[2] != "1")
            {
                Trace.WriteLine($"Usao u random broj je {c}\n");
                return -1;
            }

            return -2;
        }
    }
}
