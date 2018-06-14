using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CloudServiceData;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;

namespace WorkerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        public override void Run()
        {
            Trace.TraceInformation("WorkerRole is running");

            while (true)
            {
                string index = queueHelper.GetMessage(queueHelper.GetQueueReference("vezba7"));

                if (index == null)
                {
                    Trace.WriteLine("NemaPoruke");
                }
                else
                {
                    ResizeImage(index);
                }
                Trace.TraceInformation("Working");
                Thread.Sleep(1000);
            }
            }


        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

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

        public  Image ConvertImage(Image img)
        {
            return (Image)(new Bitmap(img, new Size(50, 50)));
        }
        public void ResizeImage(String indexNo)
        {

            Student student = tableHelper.GetOneStudent(indexNo);
            if (student == null)
            {
                Trace.TraceInformation(String.Format("Student sa brojem indeksa {0} ne postoji!", indexNo), "Information");
            return;
            }
            BlobHelper blobHelper = new BlobHelper();
            string uniqueBlobName = string.Format("image_{0}", student.RowKey);
            Image image = blobHelper.DownloadImage("kontejnerk2", uniqueBlobName);
            image = ConvertImage(image);
            string thumbnailUrl = blobHelper.UploadImage(image, "kontejnerk2", uniqueBlobName +
           "thumb");
            student.ThumbnailUrl = thumbnailUrl;
            tableHelper.AddOrReplaceStudent(student);
        }
        TableHelper tableHelper = new TableHelper();
        QueueHelper queueHelper = new QueueHelper();

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
               
               
            }
        }
    }
}
