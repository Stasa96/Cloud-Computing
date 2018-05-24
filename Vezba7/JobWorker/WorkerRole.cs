using System;
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
using static System.Net.Mime.MediaTypeNames;

namespace JobWorker
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        public override void Run()
        {
            Trace.TraceInformation("JobWorker is running");

            try
            {
                RunAsync(this.cancellationTokenSource.Token).Wait();
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

            Trace.TraceInformation("JobWorker has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("JobWorker is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("JobWorker has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
           // // TODO: Replace the following with your own logic.
           // CloudQueue queue = QueueHelper.GetQueueReference("vezba");
           // // This is a sample worker implementation. Replace with your logic.
           // Trace.TraceInformation("ImageConverter_WorkerRole entry point called",
           //"Information");

            while (true)
            {
                //CloudQueueMessage message = queue.GetMessage();
                //if (message == null)
                //{
                //    Trace.TraceInformation("Trenutno ne postoji poruka u redu.",
                //   "Information");
                //}
                //else
                //{
                //    Trace.TraceInformation(String.Format("Poruka glasi: {0}",
                //   message.AsString), "Information");
                //    ResizeImage(message.AsString);
                //    queue.DeleteMessage(message);
                //    Trace.TraceInformation(String.Format("Poruka procesuirana: {0}",
                //    message.AsString), "Information");
                //}
                
                Trace.TraceInformation("Working", "Information");
                await Task.Delay(3000);
            }
        }

        public static System.Drawing.Image ConvertImage(System.Drawing.Image img)
        {
            return (System.Drawing.Image)(new Bitmap(img, new Size(20, 20)));
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
            System.Drawing.Image image = blobHelper.DownloadImage("vezba", uniqueBlobName);
            image = ConvertImage(image);
            string thumbnailUrl = blobHelper.UploadImage(image, "vezba", uniqueBlobName +
           "thumb");
            student.ThumbnailUrl = thumbnailUrl;
            sdr.AddOrReplaceStudent(student);
        }
    }
}
