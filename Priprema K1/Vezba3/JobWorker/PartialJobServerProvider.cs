using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCFContracts;

namespace JobWorker
{
    public class PartialJobServerProvider : IPartialJob
    {
        public int DoPartialCalculus(int from,int to)
        {
            int res = 0;

            for (int i = from; i < to; i++)
            {
                res += i;
            }

            Trace.WriteLine($"Called DoSum from {from} to {to} result is {res}");

            return res;
        }
    }
}
