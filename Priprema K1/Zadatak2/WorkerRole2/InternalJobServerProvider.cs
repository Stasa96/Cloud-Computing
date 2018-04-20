using Common;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WorkerRole2
{
    public class InternalJobServerProvider : IInteraction
    {
        public string sendMsg(string msg)
        {
            Trace.WriteLine("Message: " + msg);



            return "ok";
        }
    }
}
