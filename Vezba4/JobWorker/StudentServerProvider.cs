using Contracts;
using StudentService_Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobWorker
{
    class StudentServerProvider : IStudent
    {
        StudentDataRepository rep = new StudentDataRepository();
        
        public string AddStudent(string indexNo, string name, string lastName)
        {
            List<Student>students =  rep.RetrieveAllStudents().ToList();

            Student s =  new Student(indexNo);
            s.Name = name;
            s.LastName = lastName;

            int flag = -1;

            foreach(Student st in students)
            {
                if (st.RowKey.ToUpper().Equals(indexNo.ToUpper()))
                {
                    flag = 1;
                    break;
                }
            }

            if (flag == -1)
            {
                rep.AddStudent(s);
                Trace.WriteLine($"added\n{s.ToString()}");
                return $"added\n{s.ToString()}";
            }
            Trace.WriteLine($"alredy exist\n{s.ToString()}");
            return $"alredy exist\n{s.ToString()}";
        }

        public List<Student> RetrieveAllStudents()
        {
            Trace.WriteLine("Retrieved list of students");
            return rep.RetrieveAllStudents().ToList();
        }

        public string RemoveStudent(string indexNo)
        {
            Student s = new Student(indexNo);


            List<Student> students = rep.RetrieveAllStudents().ToList();

            foreach (Student st in students)
            {
                if (st.RowKey.ToUpper().Equals(indexNo.ToUpper()))
                {
                    rep.RemoveStudent(st);
                    Trace.WriteLine($"Removed\n{st.ToString()}");
                    return $"Removed\n{st.ToString()}";
                }
            }
            Trace.WriteLine($"Doesn't exist\n{s.ToString()}");
            return $"Doesn't exist\n{s.ToString()}";
        }

        public string UpdateStudent(string indexNo, string name, string lastName)
        {
            Student s = new Student(indexNo);
            s.Name = name;
            s.LastName = lastName;


            List<Student> students = rep.RetrieveAllStudents().ToList();

            foreach (Student st in students)
            {
                if (st.RowKey.ToUpper().Equals(indexNo.ToUpper()))
                {
                    rep.UpdateStudent(s);
                    Trace.WriteLine($"Updated\n{st.ToString()}\nto\n{s.ToString()}");
                    return $"Updated\n{st.ToString()}\nto\n{s.ToString()}";
                }
            }
            Trace.WriteLine($"Doesn't exist\n{s.ToString()}");
            return $"Doesn't exist\n{s.ToString()}";
        }
    }
}
