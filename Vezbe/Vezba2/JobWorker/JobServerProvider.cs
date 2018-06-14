using InterroleContracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobWorker
{
    public class JobServerProvider : IJob
    {
        public int DoCalculus(int to)                   // implementacija interfejsa, server nudi metodu za racunanje sume prvih n brojeva
        {
            Trace.WriteLine($"DoCalculus method called - interval[1, {to}]");

            int suma = 0;
            for (int i = 0; i <= to; i++)
            {
                suma += i;
            }

            return suma;
        }
    }
}
