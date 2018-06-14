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
using WorkerRole1.InternalRequest;

namespace WorkerRole1
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
        IInteraction proxy;
        JobServer js = new JobServer();

        public override void Run()
        {
            Trace.TraceInformation("WorkerRole1 is running");

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
            js.Open();
            Trace.TraceInformation("WorkerRole1 has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("WorkerRole1 is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();
            js.Close();
            base.OnStop();

            Trace.TraceInformation("WorkerRole1 has stopped");
        }
        static int i = 0;
        private async Task RunAsync(CancellationToken cancellationToken)
        {
            

            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                Connect();
                proxy.SendCnt(++i);

                
                
                await Task.Delay(3000);
            }
        }

        private void Connect()
        {
            string name = "InputRequest";
            try
            {
                ChannelFactory<IInteraction> factory = new ChannelFactory<IInteraction>(new NetTcpBinding(), new EndpointAddress($"net.tcp://localhost:11000/{name}"));
                proxy = factory.CreateChannel();
                Trace.WriteLine("Connected on " + factory.Endpoint.Address);
                Trace.WriteLine($"Channel is opened");
            }
            catch (Exception e)
            {
                Trace.WriteLine($"Channel open error {e.Message}");
            }
        }
    }
}
