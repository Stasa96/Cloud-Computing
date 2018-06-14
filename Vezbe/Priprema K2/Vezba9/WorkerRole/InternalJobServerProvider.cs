using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerRole
{
    class InternalJobServerProvider : IBrotherConnection
    {
        public bool AreYouAlive()
        {
            return true;
        }
    }
}
