using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IStudents
    {
        [OperationContract]
        bool AddStudent(Int64 jmbg, string name, string lastName);

        
    }
}
