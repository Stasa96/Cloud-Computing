using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
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
        BlobHelper blobHelper = new BlobHelper("kontejner");
        QueueHelper queueHelper = new QueueHelper("red");
        List<TableHelper> tableHelper = new List<TableHelper>(){
            new TableHelper("recenice1"), new TableHelper("recenice2") };
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
            
            Trace.TraceInformation("WorkerRole has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("WorkerRole is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("WorkerRole has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.WriteLine("--------------------------------------");
                string msg = queueHelper.GetFromQueue();

                if(msg == null)
                {
                    Trace.WriteLine("Nema poruke");
                }
                else
                {
                    Trace.WriteLine(msg);
                    tableHelper[int.Parse(id)].AddOrReplaceRecenica(new Recenica(msg));
                    Thread.Sleep(1000);

                    string ret = "Poslato na instancu " + id + ":";
                    int i = -1;
                    foreach(TableHelper t in tableHelper)
                    {
                        ret += $"INSTANCA {++i}:";
                        t.GetAllRecenicas().ForEach(x=>
                        {
                            ret += x.Rec + ":";
                        });
                        ret += ":";
                    }

                    blobHelper.UploadStringToBlob("recenice",ret);
                }

                Trace.TraceInformation("Working");
                Trace.WriteLine("--------------------------------------");
                await Task.Delay(3000);
            }
        }
    }
}
