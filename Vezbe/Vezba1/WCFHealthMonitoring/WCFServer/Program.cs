﻿using JobWorker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCFServer
{
    class Program
    {
        static void Main(string[] args)
        {
            JobServer server = new JobServer();


            while (HealthMonitoring.brojac < 4) { }

            server.Stop();
        }
    }
}