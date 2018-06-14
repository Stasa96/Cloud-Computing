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

namespace JobWorker1
{
    public class JobServerProvider : IContract
    {
        List<IContract> proxy = new List<IContract>();
        List<Task<string>> tasks = new List<Task<string>>();

        
        public string SendMsg(string msg)
        {
            if(msg != null)
            {
                Connect();

                foreach(IContract c in proxy)
                {
                    tasks.Add(Task.Factory.StartNew(() => c.SendMsg(msg)));
                }

                Task.WaitAll(tasks.ToArray());

                foreach(Task<string> t in tasks)
                {
                    Trace.WriteLine($"From Task{t.Id} returned message: {t.Result}");
                }

                proxy.Clear();
                tasks.Clear();
                return "Uspesno poslata poruka: " + msg;
            }
            else
            {
                MyException ex = new MyException("String je null");
                throw new FaultException<MyException>(ex);
            }
        }


        private void Connect()
        {
            string name = "InternalRequest";
            int cnt = RoleEnvironment.Roles["JobWorker1"].Instances.Count;
            for (int i = 0; i < cnt; i++)
            {
                if(RoleEnvironment.CurrentRoleInstance.InstanceEndpoints[name].IPEndpoint != RoleEnvironment.Roles["JobWorker1"].Instances[i].InstanceEndpoints[name].IPEndpoint)
                {
                    RoleInstanceEndpoint endpoint = RoleEnvironment.Roles["JobWorker1"].Instances[i].InstanceEndpoints["InternalRequest"];
                    ChannelFactory<IContract> factory = new ChannelFactory<IContract>(new NetTcpBinding(), new EndpointAddress($"net.tcp://{endpoint.IPEndpoint}/{name}"));
                    proxy.Add(factory.CreateChannel());
                    Trace.WriteLine("Connected to " + name + " on " + endpoint.IPEndpoint);
                }
            }
            
        }
    }
}
