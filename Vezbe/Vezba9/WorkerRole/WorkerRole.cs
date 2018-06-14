using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CloudService_Data;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace WorkerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        static int spamCnt = 0;
        int id;

        JobServer jobServer;
        BlobHelper blobHelper;
        CloudQueue cloudQueue;
        CloudQueueMessage receivedMessage;



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

            // For information on handling configuration changes
            // see the MSDN topic at https://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            //Izvuci ID iz worker role
            id = int.Parse(RoleEnvironment.CurrentRoleInstance.Id.Split('_')[2]);
            blobHelper = new BlobHelper();
            cloudQueue = QueueHelper.GetQueueReference("queue");

            jobServer = new JobServer();

            if(id == 0)
            {
                jobServer.AddInternalService();

                try
                {
                    if(blobHelper.DownloadFromBlob("servicehoststatus").ToUpper() == "OTVORENO")
                    {
                        jobServer.OpenInternalService();
                    }
                    else
                    {
                        //POCETNO STANJE JE CLOSE
                    }
                }
                catch(Exception e)
                {
                    Trace.TraceError($"ERROR: {e.Message}");
                }
            }
            else if(id == 1)
            {
                //DRUGA INSTANCA WORKER ROLE OTVARA SERVICE HOST ZA WEB I PRIMA PORUKU
                //KONTAKTIRA PRVU INSTANCU DA PROVERI DA LI JE ZIVA
                jobServer.AddExternalService();
                jobServer.OpenExternalService();
            }
            else
            {
                Trace.WriteLine("Instance is not in use.");
            }
            Trace.TraceInformation("WorkerRole has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("WorkerRole is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            if(id == 0)
            {
                jobServer.CloseInternalService();
            }
            else if(id == 1)
            {
                jobServer.CloseExternalService();
            }
            else
            {
                Trace.WriteLine("Instance was never used");
            }
            base.OnStop();

            Trace.TraceInformation("WorkerRole has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Working");

                //PRVA INSTANCA KORISTI SAMO SERVICEHOST ZA KOMUNIKACIJU I RAD
                if (id == 1)
                {
                    
                }
                else if (id == 0)
                {
                    receivedMessage = null;

                    try
                    {
                        if ((receivedMessage = cloudQueue.GetMessage()).AsString.ToUpper().Equals("OTVORENO"))
                        {
                            Trace.WriteLine("---------------------------------------------------");
                            Trace.WriteLine("MESSAGE-> " + receivedMessage.AsString);
                            Trace.WriteLine("---------------------------------------------------");

                            OpenInternalService();
                        }
                        else if (receivedMessage.AsString.ToUpper().Equals("ZATVORENO"))
                        {
                            Trace.WriteLine("---------------------------------------------------");
                            Trace.WriteLine("MESSAGE-> " + receivedMessage.AsString);
                            Trace.WriteLine("---------------------------------------------------");

                            CloseInternalService();
                        }
                        else
                        {
                            cloudQueue.DeleteMessage(receivedMessage);
                            Trace.WriteLine("---------------------------------------------------");
                            Trace.WriteLine("MESSAGE ERROR DELETE-> " + receivedMessage.AsString);
                            Trace.WriteLine("---------------------------------------------------");


                        }
                    }
                    catch
                    {
                        if (++spamCnt == 12)
                        {
                            spamCnt = 0;
                            Trace.WriteLine("In the last 60 sec there was no messages");
                        }
                    }
                }
                else
                {
                    Trace.WriteLine("Instance is not in use.");
                }

                await Task.Delay(5000);
            }

            
        }
        private void OpenInternalService()
        {
            try
            {
                if (blobHelper.DownloadFromBlob("servicehoststatus").ToUpper().Equals("OTVORENO"))
                {
                    cloudQueue.DeleteMessage(receivedMessage);
                    Trace.WriteLine("ServiceHost was alredy opened,message deleted");
                    return;
                }
                else
                {
                    jobServer.OpenInternalService();
                    blobHelper.UploadToBlob("servicehoststatus", "OTVORENO");

                    cloudQueue.DeleteMessage(receivedMessage);
                    Trace.WriteLine("ServiceHost is opened,message deleted");
                }
            }
            catch (Exception e)
            {
                Trace.TraceError($"ERROR: {e.Message}");
                blobHelper.UploadToBlob("servicehoststatus", "ERROR");
            }
        }

        private void CloseInternalService()
        {
            try
            {
                if (blobHelper.DownloadFromBlob("servicehoststatus").ToUpper().Equals("ZATVORENO"))
                {
                    cloudQueue.DeleteMessage(receivedMessage);
                    Trace.WriteLine("ServiceHost was alredy opened,message deleted");
                    return;
                }
                else
                {
                    jobServer.CloseInternalService();
                    blobHelper.UploadToBlob("servicehoststatus", "ZATVORENO");

                    cloudQueue.DeleteMessage(receivedMessage);
                    Trace.WriteLine("ServiceHost is closed,message deleted");
                }
            }
            catch (Exception e)
            {
                Trace.TraceError($"ERROR: {e.Message}");
                blobHelper.UploadToBlob("servicehoststatus", "ERROR");
            }
        }
    }
}
