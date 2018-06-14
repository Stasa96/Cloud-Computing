using CloudService_Data;
using Common;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WorkerRole
{
    public class ExternalJobProvider : IControlServiceHost
    {
        string response = null;
        public string IsYourBrotherThere()
        {
            ConnectToBrother();

            if(response.Equals("Instance 0 is not alive."))
            {
                Recovery();
            }

            Trace.WriteLine("Instace 1 sending to web " + response);
            return response;
        }

        private string ConnectToBrother()
        {
            TableHelper table = new TableHelper();
            try
            {
                IBrotherConnection proxy = Connect();
                proxy.AreYouAlive();

                response = "Instance 0 is alive.";

                
                table.AddEvidence(new EvidenceEntity(response));
                
            }
            catch(Exception e)
            {
                response = "Instance 0 is not alive.";
                table.AddEvidence(new EvidenceEntity(response,e.Message));
            }

            return response;
        }

        private bool Recovery()
        {
            QueueHelper.GetQueueReference("queue").AddMessage(new CloudQueueMessage("OTVORENO"));
            Thread.Sleep(1000);
            ConnectToBrother();

            if(response.Equals("Instance 0 is alive."))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private IBrotherConnection Connect()
        {
            NetTcpBinding binding = new NetTcpBinding()
            {
                CloseTimeout = new TimeSpan(0, 10, 0),
                OpenTimeout = new TimeSpan(0, 10, 0),
                ReceiveTimeout = new TimeSpan(0, 10, 0),
                SendTimeout = new TimeSpan(0, 10, 0),
            };
            RoleInstanceEndpoint remoteInstanceEP = RoleEnvironment.Roles["WorkerRole"].Instances.Where(i => i.Id.Split('.').Last().Split('_').Last().Equals("0")).First().InstanceEndpoints["InternalRequest"];
            String remoteAddress = $"net.tcp://{remoteInstanceEP.IPEndpoint}/InternalRequest";
            return new ChannelFactory<IBrotherConnection>(binding, remoteAddress).CreateChannel();

        }
    }
}
