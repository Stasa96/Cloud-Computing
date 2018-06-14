using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCFContracts;

namespace JobWorker
{
    public class JobServerProvider : IJob
    {
        public int DoCalculus(int to)
        {
            int res = 0;

            for (int i = 0; i <= to; i++)
            {
                res += i;
            }
            Trace.WriteLine("Sum from 1 to " + to + " is " + res);
            return res;

            
        }
    }
}
