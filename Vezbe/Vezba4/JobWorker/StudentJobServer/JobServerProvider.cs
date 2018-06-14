using Common;
using StudentService_Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobWorker.StudentJobServer
{
    public class JobServerProvider : IStudent
    {
        StudentDataRepository repository = new StudentDataRepository();

        public void AddStudent(string index, string name, string lastName)
        {
            Student s = new Student(index);
            s.Name = name;
            s.LastName = lastName;
            
            repository.AddStudent(s);
        }

        public void EditStudent(string index, string name,string lastName)
        {
            RemoveStudent(index);
            AddStudent(index, name, lastName);
        }

        public void RemoveStudent(string index)
        {
            List <Student> students = RetrieveAllStudents();

            foreach (Student item in students)
            {
                if(item.RowKey == index)
                {
                    repository.RemoveStudent(item);
                }
            }
        }

        public List<Student> RetrieveAllStudents()
        {
            return repository.RetrieveAllStudents().ToList();

            
        }
    }
}
