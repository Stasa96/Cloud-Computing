using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCFServer
{
    public class HealthMonitoring : IHealthMonitoring
    {
        public static int brojac = 0;

        public void IAmAlive()
        {
            brojac++;

            Console.WriteLine(brojac+ ": Worker role checked in.");
        }
    }
}
