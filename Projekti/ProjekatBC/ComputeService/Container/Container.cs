using ServiceContract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Container
{
    public class Container : IContainer
    {
        public static int flag = -1;
        public string Load(string assemblyName)
        {
            string[] pom = assemblyName.Split('\\', '_');
            string port = (pom[pom.Length - 2]);

            Assembly DLL = Assembly.Load(File.ReadAllBytes(assemblyName));
            dynamic c = null;

            for (int j = 0; j < DLL.GetExportedTypes().Length; j++)
            {
                string b = DLL.GetExportedTypes()[j].Name;

                if (b == "WorkerRole")
                {
                    c = Activator.CreateInstance(DLL.GetExportedTypes()[j]);
                    c.Start(port); //provera da li ima IWorker interface
                    Console.WriteLine($"DLL {pom[pom.Length-1]} finished working.");
                }
            }
            end(assemblyName);
            return "success";
        }

        private static void end(string assemblyName)
        {
            flag = 1;
            File.Delete(assemblyName);
        }

        public string CheckState()
        {
            return "Alive";
        }
    }
}
