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
        InternalJobServer ijs;
        InputJobServer js;
        public static QueueHelper queueHelper = new QueueHelper("red");
        BlobHelper blobHelper = new BlobHelper("kontejner");
        static bool isOpen = false;
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
            if (id == "0")
                ijs = new InternalJobServer();
            else
            {
                js = new InputJobServer();
                js.Open();
            }
            Trace.TraceInformation("WorkerRole has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("WorkerRole is stopping");
            if (id == "0")
                ijs.Close();
            else
                js.Close();
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
                Trace.WriteLine("-----------------------------------------");
                //prva instanca
                if (id == "0")
                {
                    string msg = queueHelper.GetFromQueue();

                    if (msg == null)
                    {
                        Trace.WriteLine("Nema poruke");
                    }
                    else
                    {
                        Trace.WriteLine(msg);

                        if (msg == "otvori")
                        {
                            if (isOpen)
                                Trace.WriteLine("Vec je otvoreno nema potrebe ponovo otvarati");
                            else
                            {
                                blobHelper.UploadStringToBlob("stanje", "OTVORENO");
                                ijs.Open();
                                isOpen = true;
                                
                            }
                        }
                        else
                        {
                            if (!isOpen)
                                Trace.WriteLine("Vec je zatvoreno nema potrebe ponovo zatvarati");
                            else
                            {
                                blobHelper.UploadStringToBlob("stanje", "ZATVORENO");
                                ijs.Close();
                                isOpen = false;
                                
                            }
                            
                        }
                    }
                }
                //druga instanca
                else if (id == "1")
                {

                }
                Trace.TraceInformation("Working");
                Trace.WriteLine("-----------------------------------------");
                await Task.Delay(1000);
            }
        }
    }
}
