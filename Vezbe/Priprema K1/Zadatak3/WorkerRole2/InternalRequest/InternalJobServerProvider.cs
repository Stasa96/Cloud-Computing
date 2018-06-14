using Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerRole2.InternalRequest
{
    public class InternalJobServerProvider : IInteraction
    {
        public bool SendCnt(int cnt)
        {
            try
            {
               
                Trace.WriteLine($"Message: {cnt}");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
