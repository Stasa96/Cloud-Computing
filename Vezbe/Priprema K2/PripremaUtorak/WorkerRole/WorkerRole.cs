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
        public static string id = null;
        public static TableHelper tableHelper;
        QueueHelper queueHelper = new QueueHelper("red");
        BlobHelper blobHelper = new BlobHelper("filmovi");
        InputJobServer ijs = new InputJobServer();
        InternalJobServer js = new InternalJobServer();

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
            tableHelper = new TableHelper("filmovi" + id);
            // For information on handling configuration changes
            // see the MSDN topic at https://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();
            js.Open();
            ijs.Open();
            Trace.TraceInformation("WorkerRole has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("WorkerRole is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();
            ijs.Close();
            js.Close();
            base.OnStop();

            Trace.TraceInformation("WorkerRole has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            Trace.WriteLine("---------------------------------------");
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                string msg = queueHelper.GetFromQueue();

                if(msg == null)
                {
                    Trace.WriteLine("Nema poruke za novi film");
                }
                else
                {
                    Trace.WriteLine(msg);
                    Film f = new Film(msg,blobHelper.DownladPhotoURLFromBlob(msg));
                    tableHelper.AddOrReplaceFilm(f);
                }

                
               
                Trace.WriteLine("---------------------------------------");
                Trace.TraceInformation("Working");
                await Task.Delay(3000);
            }
        }
    }
}
