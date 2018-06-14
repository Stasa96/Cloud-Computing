using Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobWorker2
{
    public class JobServerProviderInternal : IContract
    {
        public string SendMsg(string msg)
        {
            Trace.WriteLine("Message: " + msg);

            return "Message is received";
        }
    }
}
