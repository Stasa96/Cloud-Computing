﻿using ServiceContract;
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
        public struct container {
            public bool state;
            public Packet packet;
        };

        #region parameters
        public struct Packet{
            public string xml;
            public string dll;
        };

        public static IContainer[] proxy = new IContainer[4];
        public static Dictionary<int, container> containers = new Dictionary<int, container>() { { 1, new container() }, { 2, new container() }, { 3, new container() }, { 4, new container() } };
        public static int instance;
        public static int containersCnt = 4;
        public static container[] niz = new container[4];
        public static Packet packet;
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
                    Console.WriteLine("EXCEPTION");
                }
            }
        }

        public static void checkPacket()
        {
            

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
            instance = readXML(packet.xml);

            if (containersCnt == 0)
            {
                Console.WriteLine("There is no containers available.");
            }
            else if (containersCnt >= instance)
            {
                checkDllImplementation();
            }
            else
            {
                Console.WriteLine("There is not enought Containers left for your aplication.");

            }
            File.Delete(packet.dll);
            File.Delete(packet.xml);
        }
        
        public static void  checkDllImplementation()
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

                    LoadDLL();
                }
            }

            if (flag == -1)
            {
                Console.WriteLine("Founded file doesn't implement IWorkerRole inteface,please implement it and add your packet again.\nPress any key to get back to meni.");
                dll = null;
            }
            Console.WriteLine("---------------------------------------------");
        }

        private static void LoadDLL()
        {
            foreach (KeyValuePair<int, container> cont in containers)
            {
                if (cont.Value.state == false && instance != 0)
                {
                    packet.dll = CopyDLL(packet.dll, cont.Key);

                    Task.Factory.StartNew(() => { proxy[cont.Key - 1].Load(packet.dll); });
                    niz[cont.Key - 1].state = true;
                    niz[cont.Key - 1].packet = packet;

                    instance--;
                    i = cont.Key;
                    containersCnt--;

                    Console.WriteLine($"sent to Container {i} with port 100{i}0");
                    Console.WriteLine("Number of available containers is: " + containersCnt);
                }
            }

            for (int j = 0; j < 4; j++)
            {
                containers[j + 1] = niz[j];
            }
            
        }

        private static string CopyDLL(string dll,int i)
        {
            string dest = $@"..\..\..\MainFolder\Port_100{i.ToString()}0";
            string destFile = dll;
            try
            {
                string sourceFile = dll;
                string[] part = dll.Split('\\');
                destFile = System.IO.Path.Combine(dest, part[part.Length - 1]);


                if (System.IO.Directory.Exists(dest))
                {
                    System.IO.File.Copy(sourceFile, destFile, true);
                }
                else
                {
                    Console.WriteLine("source or destination path doesn't exists.");
                }
            }
            catch (Exception) { }
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
               // Thread.CurrentThread.IsBackground = true;
                while (true)
                {
                    for(int j = 0;j<4;j++)
                    {
                        containersCnt = 0;
                        for (int k = 0; k < 4; k++)
                        {
                            if (containers[k + 1].state == false)
                                containersCnt++;
                        }
                        try
                        {
                            if (proxy[j].CheckState().Equals("free"))
                            {
                               
                                container c = containers[j + 1];
                                c.state = false;
                                containers[j + 1] = c;
                                
                            }
                            
                           
                            
                            
                        }
                        catch (Exception)
                        {
                            if(containersCnt > 0 && !string.IsNullOrEmpty(containers[j + 1].packet.dll))
                            {
                                Console.WriteLine($"Shutdown 100{j+1}0 new container is taking over");
                                packet.dll = containers[j + 1].packet.dll;
                                instance = 1;
                                LoadDLL();
                                //Thread.Sleep(1000);
                                container c = new container();
                                c.state = false;
                                containers[j + 1] = c;

                                
                                StartContainer((j + 1).ToString());
                                Connect(j);
                                i--;
                            }
                            else if(!string.IsNullOrEmpty(containers[j + 1].packet.dll))
                            {
                                Console.WriteLine($"Shutdown 100{j + 1}0 same container is taking over");
                                container c = containers[j + 1];
                                c.state = false;
                                containers[j + 1] = c;
                                

                                StartContainer((j + 1).ToString());
                                Connect(j);
                                

                                packet.dll = containers[j + 1].packet.dll;
                                instance = 1;
                                LoadDLL();
                            }
                        
                            
                            
                        }
                        Thread.Sleep(500);
                    }
                    
                }
            }).Start();
        }
    }
}
