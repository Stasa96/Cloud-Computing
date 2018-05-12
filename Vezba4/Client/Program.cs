using Common;
using StudentService_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            ChannelFactory<IStudent> factory = new ChannelFactory<IStudent>(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:11000/InputRequest"));

            IStudent proxy = factory.CreateChannel();


            proxy.AddStudent("pr88-2015", "Stefan", "Ruvceski");
            proxy.AddStudent("pr89-2015", "Stefana", "Ruvceska");
            proxy.AddStudent("pr90-2015", "Stefani", "Ruvceska");
            proxy.AddStudent("pr91-2015", "Stefanu", "Ruvceski");
            proxy.AddStudent("pr92-2015", "Stefa", "Ruvceska");

            foreach(Student s in proxy.RetrieveAllStudents())
            {
                Console.WriteLine(s.Index);
            }
            Console.WriteLine("-----------------------------------------------");
            proxy.RemoveStudent("pr90-2015");

            foreach (Student s in proxy.RetrieveAllStudents())
            {
                Console.WriteLine(s.Index);
            }
            Console.WriteLine("-----------------------------------------------");
            proxy.EditStudent("pr89-2015", "Teodora", "Ruvceska");

            foreach (Student s in proxy.RetrieveAllStudents())
            {
                Console.WriteLine(s.Index);
            }

            Console.ReadKey();
        }
    }
}
