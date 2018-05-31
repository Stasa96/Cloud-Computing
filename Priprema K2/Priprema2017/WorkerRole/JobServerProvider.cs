using Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerRole
{
    class JobServerProvider : INotify
    {
        public void Notify(string itemType)
        {
            Trace.WriteLine("\t\t"+itemType);
        }
    }
}
