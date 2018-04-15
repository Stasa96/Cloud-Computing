using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCFContracts;

namespace WCFServer
{
    public class HealthMonitoring : IHealthMonitoring
    {
        public string IAmAlive()
        {
           Console.WriteLine("I Am Alive");
            return "He is alive";
        }
    }
}
