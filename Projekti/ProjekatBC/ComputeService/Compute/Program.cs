using ServiceContract;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Compute
{
    class Program
    {
        #region parameters
        public struct Packet{
            public string xml;
            public string dll;
        };

        public static IContainer[] proxy = new IContainer[4];
        public static Dictionary<int, bool> portovi = new Dictionary<int, bool>() { { 1, false }, { 2, false }, { 3, false }, { 4, false } };
        static Dictionary<string, int> pom = new Dictionary<string, int>() { { "1", -1 }, { "2", -1 }, { "3", -1 }, { "4", -1 } };
        public static int containersCnt = 4;

        public static Process[] processes = new Process[4];
        public static string path = @"..\..\..\MainFolder\FolderForChecking";

        public static List<string> XMLs = new List<string>();
        public static List<string> DLLs = new List<string>();
        static int i = 0;
        #endregion
        static void Main(string[] args)
        {
            StartContainers();
            
            ConnectAll();
            
            CheckState();
            meni();

            Console.WriteLine("Compute is closed...");
            Console.ReadKey();
        }

        public static void StartContainers()
        {
            for (int i = 1; i <= 4; i++)
            {
                StartContainer(i.ToString());
                Console.WriteLine($"Container {i} is opened.");
            }
            ;
        }

        public static void StartContainer(string containerId)
        {
            processes[int.Parse(containerId) - 1] = new Process();
            processes[int.Parse(containerId) - 1].StartInfo.Arguments = $"100{containerId}0";
            processes[int.Parse(containerId) - 1].StartInfo.FileName = @"..\..\..\Container\bin\Debug\Container.exe";
            processes[int.Parse(containerId) - 1].EnableRaisingEvents = true;
            processes[int.Parse(containerId) - 1].Start();
        }

        public static void Connect(int i)
        {
            try
            {
                ChannelFactory<IContainer> factory1 = new ChannelFactory<IContainer>(new NetTcpBinding(), new EndpointAddress($"net.tcp://localhost:100{i+1}0/IContainer"));
                proxy[i] = factory1.CreateChannel();
            }
            catch (Exception)
            {
                Console.WriteLine("\nProxy couldn't Connect to container.");
            }
            Console.WriteLine("---------------------------------------------");
        }
        public static void ConnectAll()
        {
            try
            {
                ChannelFactory<IContainer> factory1 = new ChannelFactory<IContainer>(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:10010/IContainer"));
                proxy[0] = factory1.CreateChannel();

                ChannelFactory<IContainer> factory2 = new ChannelFactory<IContainer>(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:10020/IContainer"));
                proxy[1] = factory2.CreateChannel();

                ChannelFactory<IContainer> factory3 = new ChannelFactory<IContainer>(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:10030/IContainer"));
                proxy[2] = factory3.CreateChannel();

                ChannelFactory<IContainer> factory4 = new ChannelFactory<IContainer>(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:10040/IContainer"));
                proxy[3] = factory4.CreateChannel();
             
                Console.WriteLine("\nproxy Connected to all 4 containers.");
                Console.WriteLine("\nNumber of available containers is: " + containersCnt);
            }
            catch (Exception)
            {
                Console.WriteLine("\nProxy couldn't Connect to all 4 containers.");
            }
            Console.WriteLine("---------------------------------------------");
        }

        public static void meni()
        {
            string answer = "";
            while (answer != "x")
            {
                Console.WriteLine("Press any key to check packets or x to exit.");
                try
                {
                    answer = Console.ReadKey().KeyChar.ToString();
                    Console.WriteLine();
                    if(answer == "x")
                    {
                        break;
                    }
                    else
                    {
                        checkPacket();
                    }
                }
                catch (Exception)
                {

                }
            }
        }

        public static void checkPacket()
        {
            Packet packet;

            Console.Clear();
            Console.WriteLine("Folder is:");
            while (DLLs.Count < 1 || XMLs.Count < 1)
            {
                DLLs = Directory.GetFiles(path, "*.dll").ToList<string>();
                XMLs = Directory.GetFiles(path, "*.xml").ToList<string>();
                Console.WriteLine("empty");
                Thread.Sleep(2000);
            }

            Console.WriteLine("not empty");
            Console.WriteLine("---------------------------------------------");

            foreach (string xml in XMLs)
            {
                string[] temp1 = xml.Split('.','\\');

                foreach(string dll in DLLs)
                {
                    string[] temp2 = dll.Split('.', '\\');

                    if(String.Compare(temp1[temp1.Count() - 2], temp2[temp2.Count() - 2]) == 0)
                    {
                        packet.xml = xml;
                        packet.dll = dll;

                        CheckContainerCapacity(packet);
                    }
                }
            }
            XMLs.Clear();
            XMLs.Clear();
        }

        public static void CheckContainerCapacity(Packet packet)
        {
            int instace = readXML(packet.xml);

            if (containersCnt == 0)
            {
                Console.WriteLine("There is no containers available.");
            }
            else if (containersCnt >= instace)
            {
                checkDllImplementation(instace, packet);
            }
            else
            {
                Console.WriteLine("There is not enought Containers left for your aplication.");

            }
            File.Delete(packet.dll);
            File.Delete(packet.xml);
        }
        
        public static void  checkDllImplementation(int instance,Packet packet)
        {            
            Assembly dll = Assembly.Load(File.ReadAllBytes(packet.dll));
            int flag = -1;

            foreach (var type in dll.GetTypes())
            {
                
                var myInterfaceType = typeof(IWorkerRole);
                if (type.GetInterfaces().Contains(typeof(IWorkerRole)))
                {
                    Console.WriteLine($"class: {type}\nDLL: {packet.dll.Split('\\','.')[packet.dll.Split('\\','.').Length-2]}\nimplements interface: IWorkerRole");
                    flag = 1;
                    foreach (KeyValuePair<int, bool> port in portovi)
                    {
                        if (port.Value == false && instance != 0)
                        {
                            packet.dll = CopyDLL(packet.dll,port.Key);

                            Task.Factory.StartNew(() => { pom[proxy[port.Key - 1].Load(packet.dll)] = 0; });
                            pom[(++i).ToString()] = port.Key;
                            
                            instance--;
                            containersCnt--;

                            Console.WriteLine($"sent to Container {i} with port 100{i}0");
                            Console.WriteLine("Number of available containers is: " +containersCnt);
                        }
                    }
                    for (int j = 1; j <= 4; j++)
                    {
                        if (pom[j.ToString()] != -1)
                        {
                            portovi[pom[j.ToString()]] = true;

                            if(pom[j.ToString()] == 0)
                            {
                                portovi[pom[j.ToString()]] = false;
                            }
                        }
                    }
                }
            }

            if (flag == -1)
            {
                Console.WriteLine("Founded file doesn't implement IWorkerRole inteface,please implement it and add your packet again.\nPress any key to get back to meni.");
                dll = null;
            }
            Console.WriteLine("---------------------------------------------");
        }

        private static string CopyDLL(string dll,int i)
        {
            string dest = $@"..\..\..\MainFolder\Port_100{i.ToString()}0";

            string sourceFile = dll;
            string[] part = dll.Split('\\');
            string destFile = System.IO.Path.Combine(dest, part[part.Length - 1]);

            if (System.IO.Directory.Exists(dest))
            {
                System.IO.File.Copy(sourceFile, destFile, true);
            }
            else
            {
                Console.WriteLine("source or destination path doesn't exists.");
            }
            
            return destFile;
        }

        public static int readXML(string path)
        {
            int instances = -1;

            using (XmlReader reader = XmlReader.Create(path))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        switch (reader.Name)
                        {
                            case ("Instances"):
                                instances = int.Parse(reader.ReadElementContentAsString());
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            return instances;
        }

        public static void CheckState()
        {
            new Thread(() =>
            {
                //Thread.CurrentThread.IsBackground = true;
                while (true)
                {
                    for(int j = 0;j<4;j++)
                    {
                        try
                        {
                            proxy[j].CheckState().Equals("Alive");
                            
                        }
                        catch (Exception)
                        {
                            portovi[j+1] = false;
                            containersCnt++;
                            pom[(i + 1).ToString()] = 0;
                            StartContainer((j+1).ToString());
                            Connect(j);
                            i--;
                        }
                    }
                    Thread.Sleep(1000);
                }
            }).Start();
        }
    }
}
