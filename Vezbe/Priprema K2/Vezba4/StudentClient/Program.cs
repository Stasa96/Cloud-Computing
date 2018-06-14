using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StudentClient
{
    class Program
    {
        static void Main(string[] args)
        {
            ChannelFactory<IStudents> factory = new ChannelFactory<IStudents>(new  NetTcpBinding(),new EndpointAddress("net.tcp://localhost:8888/InputRequest"));
            IStudents proxy = factory.CreateChannel();
            int i = 5;
            while (true)
            {
                if(proxy.AddStudent(++i, $"ime{i}", $"prezime{i}"))
                {
                    Console.WriteLine("Student added");
                }
                else
                {
                    Console.WriteLine("Student not added");
                }
                
                Thread.Sleep(5000);
            }
        }
    }
}
