using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobWorker
{
    class StudentServerProvider : IStudent
    {
        public void AddStudent(string indexNo, string name, string lastName)
        {
            throw new NotImplementedException();
        }

        public List<Student> RetrieveAllStudents()
        {
            throw new NotImplementedException();
        }
    }
}
