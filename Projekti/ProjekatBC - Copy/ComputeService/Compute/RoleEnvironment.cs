using ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compute
{
    public class RoleEnvironment : IRoleEnvironment
    {
        // TO DO
        public string[] BrotherInstances(string myAssemblyName, string myAddress)
        {
            throw new NotImplementedException();
        }

        public string GetAddress(string myAssemblyName, string containerId)
        {
            throw new NotImplementedException();
        }
    }
}
