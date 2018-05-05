using ServiceContract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Container
{
    public class Container : IContainer
    {
        
        public string Load(string assemblyName)
        {
            Program.flag = 1;
            
            string[] pom = assemblyName.Split('\\', '_');
            string port = Program.port;

            Assembly DLL = Assembly.Load(File.ReadAllBytes(assemblyName));
            dynamic c = null;

            for (int j = 0; j < DLL.GetExportedTypes().Length; j++)
            {
                string b = DLL.GetExportedTypes()[j].Name;

                if (b == "WorkerRole")
                {
                    c = Activator.CreateInstance(DLL.GetExportedTypes()[j]);
                    new Thread(() =>
                    {
                        //Thread.CurrentThread.IsBackground = true;
                        c.Start(port);
                        Thread.Sleep(1000);
                        end(assemblyName,port);
                    }).Start();
                    
                    
                    
                }
            }
            //end(assemblyName);
            return "success";
        }

        private static void end(string assemblyName,string port)
        {
            Program.flag = -1;
            Console.WriteLine("end");
            Console.Clear();
            Console.WriteLine("Service host is open on " + port + " port.\n_______________________________________________");
            //File.Delete(assemblyName);
        }

        public string CheckState()
        {
            if(Program.flag == -1)
            {
                return "free";
            }
            else
            {
                return "notfree";
            }
        }
    }
}
