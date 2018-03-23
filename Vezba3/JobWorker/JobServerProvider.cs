using InterroleContracts;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JobWorker
{
    public class JobServerProvider : IJob
    {
        private static Dictionary<string, IPartialJob> proxy = new Dictionary<string, IPartialJob>();
        private static Dictionary<string, Task<int>> tasks = new Dictionary<string, Task<int>>();
        public int DoCalculus(int to)                   // implementacija interfejsa, server nudi metodu za racunanje sume prvih n brojeva
        {
            Trace.WriteLine($"DoCalculus method called - interval[1, {to}]");

            Connect();

            int step = (int)Math.Ceiling( (double)to/(double)proxy.Count);

            int from=0;
            int too = step;
            int cnt = 0;

            foreach(KeyValuePair<string, IPartialJob> p in proxy)
            {
                if (cnt < proxy.Count-1)
                {
                    Trace.WriteLine($"Calling {p.Key} from {from} to {too}");
                    if (!tasks.ContainsKey(p.Key))
                    {
                        tasks.Add(p.Key, Task.Factory.StartNew(() => p.Value.DoSum(from, too)));
                    }
                    else
                    {
                        tasks[p.Key] = Task.Factory.StartNew(() => p.Value.DoSum(from, too));
                    }
                    Thread.Sleep(500);
                    from = too;
                    too += step;
                    cnt++;
                }
                else
                {
                    too = to+1;
                    Trace.WriteLine($"Calling {p.Key} from {from} to {too}");
                    if (!tasks.ContainsKey(p.Key))
                    {
                        tasks.Add(p.Key, Task.Factory.StartNew(() => p.Value.DoSum(from, too)));
                    }
                    else
                    {
                        tasks[p.Key] = Task.Factory.StartNew(() => p.Value.DoSum(from, too));
                    }
                    Thread.Sleep(500);
                }

                
            }

            Task.WaitAll(tasks.Values.ToArray());

            int res = 0;

            foreach(KeyValuePair<string,Task<int>> t in tasks)
            {
                res += t.Value.Result;
                Trace.WriteLine($"Result from {t.Key}  is {t.Value.Result}");
                
            }

            Trace.WriteLine($"Sum of DoCalculus us {res}");

            

            return res;
        }

        public static void Connect()
        {
            int internalServers = RoleEnvironment.Roles["JobWorker"].Instances.Count();
            IPartialJob pom;


            for (int i = 0; i < internalServers; i++)
            {
                if (RoleEnvironment.Roles["JobWorker"].Instances[i].Id != RoleEnvironment.CurrentRoleInstance.Id && !proxy.ContainsKey(RoleEnvironment.Roles["JobWorker"].Instances[i].Id))
                {
                    var binding = new NetTcpBinding();
                    IPEndPoint ipAddress = RoleEnvironment.Roles["JobWorker"].Instances[i].InstanceEndpoints["InternalRequest"].IPEndpoint;
                    ChannelFactory<IPartialJob> factory = new ChannelFactory<IPartialJob>(binding, new EndpointAddress($"net.tcp://{ipAddress}/InternalRequest"));     // ovde mora biti InputRequest!
                    pom = factory.CreateChannel();
                    proxy.Add(RoleEnvironment.Roles["JobWorker"].Instances[i].Id,pom);
                }
            }
        }
    }


}
