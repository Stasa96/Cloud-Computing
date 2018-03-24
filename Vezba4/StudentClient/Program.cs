using Contracts;
using StudentService_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading;
using System.Xml;
using System.IO;

namespace StudentClient
{
    public class Program
    {
        static IStudent proxy;
        public static void Main(string[] args)
        {
            Connect();
            meni();
        }

        private static void Connect()
        {
            ChannelFactory<IStudent> factory = new ChannelFactory<IStudent>(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:8000/InputRequest"));
            proxy = factory.CreateChannel();
        }

        private static void meni()
        {
            char ch = '9';
            while (!ch.Equals('0'))
            {
                Console.Clear();
                Console.WriteLine("________Meni_________");
                Console.WriteLine("|1.ADD");
                Console.WriteLine("|2.REMOVE");
                Console.WriteLine("|3.UPDATE");
                Console.WriteLine("|4.RETRIEVE_ALL");
                Console.WriteLine("|5.XML TO DATABASE");
                Console.WriteLine("|0.EXIT");
                Console.WriteLine("_____________________");
                ch = Console.ReadKey().KeyChar;
                Console.Clear();
                switch (ch)
                {
                    
                    case '1':
                        Add();
                        break;
                    case '2':
                        Remove();
                        break;
                    case '3':
                        Update();
                        break;
                    case '4':
                        RetrieveAll();
                        break;
                    case '5':
                        XmlToDatabase();
                        break;
                    case '0':
                        Add();
                        break;
                    default:
                        break;
                }
                Thread.Sleep(2000);
            }
        }

        private static void Add()
        {
            Console.WriteLine("___________________________________");
            Console.Write("Enter indeks: ");
            Student s = new Student(Console.ReadLine().ToUpper());

            Console.WriteLine("___________________________________");
            Console.Write("Enter first name: ");
            s.Name = Console.ReadLine().ToUpper();

            Console.WriteLine("___________________________________");
            Console.Write("Enter last name: ");
            s.LastName = Console.ReadLine().ToUpper();

            Console.WriteLine(proxy.AddStudent(s.RowKey, s.Name, s.LastName));
        }

        private static void Remove()
        {
            Console.WriteLine("___________________________________");
            Console.Write("Enter indeks: ");
            string ind = Console.ReadLine().ToUpper();

            Console.WriteLine(proxy.RemoveStudent(ind));
        }

        private static void Update()
        {
            Console.WriteLine("___________________________________");
            Console.Write("Enter indeks: ");
            Student s = new Student(Console.ReadLine().ToUpper());

            Console.WriteLine("___________________________________");
            Console.Write("Enter first name: ");
            s.Name = Console.ReadLine().ToUpper();

            Console.WriteLine("___________________________________");
            Console.Write("Enter last name: ");
            s.LastName = Console.ReadLine().ToUpper();

            Console.WriteLine(proxy.UpdateStudent(s.RowKey, s.Name, s.LastName));
        }

        private static void RetrieveAll()
        {
            List<Student> students = proxy.RetrieveAllStudents().ToList();
            foreach (Student student in students)
            {
                Console.WriteLine("___________________________________");
                Console.WriteLine(student);

            }
            CreateXMLfromDatabase(students);
        }

        private static void CreateXMLfromDatabase(List<Student> students)
        {
            XmlWriterSettings set = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "\t",
                NewLineOnAttributes = true
            };

            using (XmlWriter writer = XmlWriter.Create(@"..\..\..\Students.xml", set))
            {
                writer.WriteStartDocument();
                    writer.WriteStartElement("Students");
                        foreach (Student student in students)
                        {
                            writer.WriteStartElement(student.PartitionKey);
                                writer.WriteElementString("Indeks",student.RowKey);
                                writer.WriteElementString("FirstName", student.Name);
                                writer.WriteElementString("LastName", student.LastName);
                            writer.WriteEndElement();
                        }
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        private static void XmlToDatabase()
        {
            Console.WriteLine("___________________________________");
            Console.Write("Enter xml file name: ");
            AddStudentsToDataBaseFromXML(Console.ReadLine().ToLower());
        }

        private static void AddStudentsToDataBaseFromXML(string xml)
        {
            List<string> files = Directory.GetFiles(@"..\..\..\").ToList();

            foreach(string file in files)
            {
                string[] pom1 = file.Split('\\', '.');
                string[] pom2 = xml.Split('.');

                if(pom1[pom1.Length-2].ToLower() == pom2[0].ToLower())
                {
                    LoadStudents(file);
                }
            }
        }

        private static void LoadStudents(string xmlFile)
        {
            Student s = new Student();

            using (XmlReader reader = XmlReader.Create(xmlFile))
            {
                while (reader.Read())
                {
                    if(reader.NodeType == XmlNodeType.Element)
                    {
                        switch (reader.Name)
                        {
                            case "Indeks":
                                s.RowKey = reader.ReadElementContentAsString();
                                break;

                            case "FirstName":
                                s.Name = reader.ReadElementContentAsString();
                                break;
                            case "LastName":
                                s.LastName = reader.ReadElementContentAsString();
                                proxy.AddStudent(s.RowKey,s.Name,s.LastName);
                                s = new Student();
                                break;
                            default:
                                break;
                        }

                    }
                }
            }
            Console.WriteLine("Loading students to Database is finished");
        }
    }
}
