using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using StorageHelper;

namespace WorkerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
        string id = null;
        QueueHelper queueHelper = new QueueHelper("red");
        TableHelper tableHelper = new TableHelper("prekidaci");
        INotify proxy;
        JobServer js;

        public override void Run()
        {
            Trace.TraceInformation("WorkerRole is running");

            try
            {
                this.RunAsync(this.cancellationTokenSource.Token).Wait();
            }
            finally
            {
                this.runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;
            id = RoleEnvironment.CurrentRoleInstance.Id.Split('_')[2];
            // For information on handling configuration changes
            // see the MSDN topic at https://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();
            if(id == "1")
            {
                js = new JobServer();
                js.Open();
            }
            else
            {
                ConnectToHost();
            }

            Trace.TraceInformation("WorkerRole has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("WorkerRole is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();
            if(id == "1")
            {
                js.Close();
            }
            base.OnStop();

            Trace.TraceInformation("WorkerRole has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.WriteLine("--------------------------------");
                if(id == "0")
                {
                    string msg = queueHelper.GetFromQueue();

                    if (msg == null)
                    {
                        Trace.WriteLine("Nema poruke");
                    }
                    else
                    {
                        if (msg.Split(':')[1] == "zatvoreno")
                        {
                            Trace.WriteLine(msg);
                            string[] parts = msg.Split(':');
                            Prekidac p = new Prekidac(parts[0], parts[1]);

                            tableHelper.AddOrReplacePrekidac(p);
                        }
                        else
                        {
                            Trace.WriteLine("Send To Instance1");
                            proxy.Notify(msg);
                        }
                    }

                }
                

                Trace.TraceInformation("Working");
                Trace.WriteLine("--------------------------------");
                await Task.Delay(3000);
            }
        }
        private void ConnectToHost()
        {
            IPEndPoint add = RoleEnvironment.Roles["WorkerRole"].Instances[1].InstanceEndpoints["InternalRequest"].IPEndpoint;
            ChannelFactory<INotify> factory = new ChannelFactory<INotify>(new NetTcpBinding(), new EndpointAddress($"net.tcp://{add}/InternalRequest"));
            proxy = factory.CreateChannel();
        }
    }
}
