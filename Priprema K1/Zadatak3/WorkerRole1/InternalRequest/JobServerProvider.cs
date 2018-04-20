using Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WorkerRole1.InternalRequest
{
    public class JobServerProvider : IInteraction1
    {
        IInteraction proxy;
        public void SendAgain(string s, int i)
        {
            if(s.ToLower().Equals("ponovi transakciju"))
            {

                Connect();
                proxy.SendCnt(i);
                Trace.WriteLine("Saljem ponovo " + i);
            }
        }

        private void Connect()
        {
            ChannelFactory<IInteraction> factory = new ChannelFactory<IInteraction>(new NetTcpBinding(),new EndpointAddress("net.tcp://localhost:11000/InputRequest"));
            proxy = factory.CreateChannel();
        }
    }
}
