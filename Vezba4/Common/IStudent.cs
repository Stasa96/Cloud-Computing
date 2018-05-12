using StudentService_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IStudent
    {
        [OperationContract]
        List<Student> RetrieveAllStudents();

        [OperationContract]
        void RemoveStudent(string index);
        [OperationContract]
        void EditStudent(string index, string name, string lastName);
        [OperationContract]
        void AddStudent(string index, string name, string lastName);
    }
}
