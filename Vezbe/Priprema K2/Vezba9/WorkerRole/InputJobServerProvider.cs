using Common;
using Microsoft.WindowsAzure.ServiceRuntime;
using StorageHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WorkerRole
{
    public class InputJobServerProvider : IBrotherConnection
    {
        List<IBrotherConnection> proxy = new List<IBrotherConnection>();
        TableHelper tableHelper = new TableHelper("vezba9");

        public bool AreYouAlive()
        {
            try
            {
                bool retVal = false;
                Connect();
                proxy.ForEach(x => { retVal = x.AreYouAlive(); });
                tableHelper.AddOrReplaceStanje(new Stanje("OTVORENO"));
                return retVal;
            }
            catch
            {
                tableHelper.AddOrReplaceStanje(new Stanje("ZATVORENO"));
                WorkerRole.queueHelper.AddToQueue("otvori");
                return false;
            }
        }

        private void Connect()
        {
            
            foreach (RoleInstance r in RoleEnvironment.Roles["WorkerRole"].Instances)
            {
                if (r.Id.Split('_')[2] != WorkerRole.id)
                {
                    IPEndPoint add = r.InstanceEndpoints["InternalRequest"].IPEndpoint;
                    ChannelFactory<IBrotherConnection> factory = new ChannelFactory<IBrotherConnection>(new NetTcpBinding(), new EndpointAddress($"net.tcp://{add}/InternalRequest"));
                    proxy.Add(factory.CreateChannel());
                }
            }
            
        }
    }
}
