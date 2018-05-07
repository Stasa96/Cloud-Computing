﻿using ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Container
{
    class Program
    {
        #region parameters
        static ServiceHost svc;
        public static int flag = -1;
        public static string port;
        static string ContainerId;
        static IRoleEnvironment proxy;
        public static  string assemblyName;
        #endregion parameters

        #region Main
        static void Main(string[] args)
        {
            ContainerId = args[0];

            ChannelFactory<IRoleEnvironment> factory = new ChannelFactory<IRoleEnvironment>(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:11001/IRoleEnvironment"));
            proxy = factory.CreateChannel();
     

            OpenServiceHost(port = proxy.GetAddress(ContainerId));
            BrotherInstance();

            Console.ReadKey();
            
        }
        #endregion Main
        
        #region ProjekatA
        public static void OpenServiceHost(string port)
        {
            svc = new ServiceHost(typeof(Container));
            svc.AddServiceEndpoint(typeof(IContainer),
            new NetTcpBinding(),
            new Uri($"net.tcp://localhost:{port}/IContainer"));

            svc.Open();
            Console.WriteLine("Service host is open on " + port + " port.\n_______________________________________________");
        }
        public static void CloseServiceHost()
        {
            svc.Close();
        }
        #endregion ProjekatA
        
        #region ProjekatBC
        private static void BrotherInstance()
        {
            new Thread(() =>
            {
                // Thread.CurrentThread.IsBackground = true;
                while (true)
                {
                    foreach(string s in proxy.BrotherInstances(assemblyName, port))
                    {
                        Console.WriteLine(s);
                    }
                    Thread.Sleep(5000);
                }

            }).Start();
        }
        #endregion ProjekatBC
    }
}
