﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using StudentsService_Data;

//Omogućiti da se poruka generiše 30 sekundi od dodavanja studenta. 
//Ukoliko se u međuvremenu student obriše, poruka će biti “zarazna”.
//Sta tacno znaci?????????????????

namespace ImageConverter_WorkerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        public override void Run()
        {
            CloudQueue queue = QueueHelper.GetQueueReference("vezba");
            
            Trace.TraceInformation("ImageConverter_WorkerRole entry point called","Information");

            while (true)
            {
                
                // pokusaj preuzimanja poruke sa Queue
                CloudQueueMessage message = queue.GetMessage();
                if (message == null)
                {
                    Trace.TraceInformation("Trenutno ne postoji poruka u redu.",
                   "Information");
                }
                else
                {
                    
                    Trace.TraceInformation(String.Format("Poruka glasi: {0}",
                    message.AsString), "Information");

                    if (!message.AsString.Split('-')[0].Substring(0, 2).Equals("PR"))
                    {
                        Trace.WriteLine("=================" + message.DequeueCount + "=================");
                        if (message.DequeueCount == 3)
                        {
                            queue.DeleteMessage(message);
                            Trace.WriteLine("Poruka zarazena i izbrisana posle 3 pokusaja prihvatanja");
                            
                        }
                    }
                    else
                    {
                        // resize slike ako je veca od specificirane velicine
                        ResizeImage(message.AsString);

                        //brisanje poruke iz Queue
                        queue.DeleteMessage(message);

                        Trace.TraceInformation(String.Format("Poruka procesuirana: {0}",
                        message.AsString), "Information");
                    }
                }
                Thread.Sleep(5000);
                Trace.TraceInformation("Working", "Information");
            }
        }

        public void ResizeImage(String indexNo)
        {
            StudentDataRepository sdr = new StudentDataRepository();
            Student student = sdr.GetStudent(indexNo);
            if (student == null)
            {
                Trace.TraceInformation(String.Format("Student sa brojem indeksa {0} ne postoji!", indexNo), "Information");
            return;
            }
            BlobHelper blobHelper = new BlobHelper();
            string uniqueBlobName = string.Format("image_{0}", student.RowKey);
            Image image = blobHelper.DownloadImage("vezba", uniqueBlobName);
            image = ImageConvertes.ConvertImage(image);
            string thumbnailUrl = blobHelper.UploadImage(image, "vezba", uniqueBlobName +
           "thumb");
            student.ThumbnailUrl = thumbnailUrl;
            sdr.AddOrReplaceStudent(student);
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at https://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("ImageConverter_WorkerRole has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("ImageConverter_WorkerRole is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("ImageConverter_WorkerRole has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Working");
                await Task.Delay(1000);
            }
        }
    }
}
