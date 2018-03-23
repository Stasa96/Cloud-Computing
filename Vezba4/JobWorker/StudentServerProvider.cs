using Contracts;
using StudentService_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobWorker
{
    class StudentServerProvider : IStudent
    {
        StudentDataRepository rep = new StudentDataRepository();
        
        public void AddStudent(string indexNo, string name, string lastName)
        {
            List<Student>st =  rep.RetrieveAllStudents().ToList();

            // OVDE PROVERITI LOGIKU

            Student s =  new Student(indexNo);
            s.Name = name;
            s.LastName = lastName;

            int flag = -1;

            foreach(Student ss in st)
            {
                if (ss.RowKey.Equals(s.RowKey))
                {
                    flag = 1;
                    break;
                }
            }

            if(flag == -1)
                rep.AddStudent(s);

            flag = -1;
        }

        public List<Student> RetrieveAllStudents()
        {
            return rep.RetrieveAllStudents().ToList();
        }

        public string RemoveStudent(string indexNo)
        {
            Student s = new Student(indexNo);


            List<Student> students = rep.RetrieveAllStudents().ToList();

            foreach (Student st in students)
            {
                if (st.RowKey.Equals(indexNo))
                {
                    rep.RemoveStudent(s);
                    return "Success";
                }
            }
            return "Fail";
        }

        public string UpdateStudent(string indexNo, string name, string lastName)
        {
            Student s = new Student(indexNo);
            s.Name = name;
            s.LastName = lastName;


            List<Student> students = rep.RetrieveAllStudents().ToList();

            foreach (Student st in students)
            {
                if (st.RowKey.Equals(indexNo))
                {
                    rep.UpdateStudent(s);
                    return "Success";
                }
            }
            return "Fail";
        }
    }
}
