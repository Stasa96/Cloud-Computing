using Contracts;
using StudentService_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace StudentClient
{
    class Program
    {
        static void Main(string[] args)
        {
            
            ChannelFactory<IStudent> factory = new ChannelFactory<IStudent>(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:8000/InputRequest"));

            IStudent proxy = factory.CreateChannel();

            proxy.AddStudent("PR88-2015","Stefan","Ruvceski");
            proxy.AddStudent("PR86-2015", "Marko", "Pejic");
            proxy.AddStudent("PR76-2015", "Markolina", "Pejic");
            proxy.AddStudent("PR56-2015", "Markic", "Pejic");

            List<Student> studenti = proxy.RetrieveAllStudents();

            proxy.RemoveStudent("PR86-2015");
            studenti = proxy.RetrieveAllStudents();

            //proxy.UpdateStudent("PR88-2015", "Teodora", "Ruvceski");


            foreach (Student s in studenti)
            {
                Console.WriteLine(s.ToString());
            }
            Console.ReadKey();
            
        }
    }
}
