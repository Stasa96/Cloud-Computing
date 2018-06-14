using Common;
using StorageHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerRole
{
    public class JobServerProvider : INotify
    {
        TableHelper tableHelper = new TableHelper("prekidaci");
        public void Notify(string prekidac)
        {
            Trace.WriteLine(prekidac);
            string[] parts = prekidac.Split(':');
            Prekidac p = new Prekidac(parts[0], parts[1]);

            tableHelper.AddOrReplacePrekidac(p);
        }
    }
}
