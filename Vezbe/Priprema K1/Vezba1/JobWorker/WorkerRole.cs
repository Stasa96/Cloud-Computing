using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using WCFContracts;

namespace JobWorker
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
        IHealthMonitoring proxy;
        int i = 0;

        public override void Run()
        {
            Trace.TraceInformation("JobWorker is running");

            try
            {
                this.RunAsync(this.cancellationTokenSource.Token).Wait();
            }
            catch(Exception)
            {
                OnStop();
               
            }
            //finally
            //{
            //    this.runCompleteEvent.Set();
            //}
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at https://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();
            Connect();
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
            
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Working");
                if(++i > 5) { break; }
                Trace.WriteLine( proxy.IAmAlive());
                await Task.Delay(5000);
            }
        }

        public void Connect()
        {
            ChannelFactory<IHealthMonitoring> factory = new ChannelFactory<IHealthMonitoring>(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:11000/HealthMonitoring"));

            proxy = factory.CreateChannel();

            Console.WriteLine("Connected.");
        }
    }
}
