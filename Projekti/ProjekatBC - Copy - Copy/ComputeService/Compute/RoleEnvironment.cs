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
        // TO DO(Proveriti GetAddress metodu zasto assemblyName)
        #region parameters
        public struct Packet
        {
            public string xml;
            public string dll;
        };
        public struct container
        {
            public bool state;
            public Packet packet;
        };

        public static Dictionary<int, container> containers = new Dictionary<int, container>() { { 1, new container() }, { 2, new container() }, { 3, new container() }, { 4, new container() } };
        #endregion parameters
        public string[] BrotherInstances(string myAssemblyName, string myAddress)
        {
            List<string> retVal = new List<string>();
            
            foreach (KeyValuePair<int, container> c in containers)
            {

                if (!string.IsNullOrEmpty(c.Value.packet.dll) && !string.IsNullOrEmpty(myAssemblyName))
                {
                    string[] string1 = c.Value.packet.dll.Split('\\');
                    string[] string2 = myAssemblyName.Split('\\');
                    if (string1[string1.Length - 1].Equals(string2[string2.Length - 1]) && !myAddress.Equals($"100{c.Key}0"))
                        retVal.Add($"100{c.Key}0");
                }
            }

            return retVal.ToArray();
        }

        public string GetAddress( string containerId)
        {
            return $"100{containerId}0";
        }
    }
}
