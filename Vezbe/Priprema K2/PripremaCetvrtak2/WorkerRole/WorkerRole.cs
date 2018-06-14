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
        TableHelper tableHelper = new TableHelper("proizvodi");
        INotify proxy;
        JobServer js = new JobServer();
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
            id = RoleEnvironment.CurrentRoleInstance.Id.Split('_')[2];
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at https://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();
            if (id == "0")
                CreateChannel();
            else
                js.Open();
            Trace.TraceInformation("WorkerRole has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("WorkerRole is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();
            if (id == "1")
                js.Close();
            base.OnStop();

            Trace.TraceInformation("WorkerRole has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.WriteLine("----------------------------------------------");
                if(id == "0")
                {
                    string msg = queueHelper.GetFromQueue();

                    if(msg != null)
                    {
                        try
                        {
                            string[] parts = msg.Split('_');
                            Proizvod a = tableHelper.GetOneProizvod(parts[0]);
                            if(a.Kolicina >= int.Parse(parts[1]))
                            {
                                
                                Trace.WriteLine($"OK\n{a.Naziv}\n{a.Kolicina}\n{parts[1]}");
                                a.Kolicina -= int.Parse(parts[1]);
                                tableHelper.AddOrReplaceProizvod(a);
                                proxy.Notify(msg+"_OK");
                                Trace.WriteLine($"OK\n{a.Naziv}\n{a.Kolicina}\n{parts[1]}");
                            }
                            else
                            {
                                proxy.Notify(msg + "_NOTOK");
                                Trace.WriteLine("Nema dovoljno proizvoda.");
                            }
                        }
                        catch
                        {
                            proxy.Notify(msg + "_NOTOK");
                            Trace.WriteLine($"NOT OK");
                        }


                    }
                    else
                    {
                        Trace.WriteLine("Nema poruke");
                    }
                }
                Trace.TraceInformation("Working");
                Trace.WriteLine("----------------------------------------------");
                await Task.Delay(3000);
            }
        }
        private void CreateChannel()
        {
            IPEndPoint add = RoleEnvironment.Roles["WorkerRole"].Instances[1].InstanceEndpoints["InternalRequest"].IPEndpoint;
            ChannelFactory<INotify> factory = new ChannelFactory<INotify>(new NetTcpBinding(), new EndpointAddress($"net.tcp://{add}/InternalRequest"));
            proxy = factory.CreateChannel();
        }
    }
}
