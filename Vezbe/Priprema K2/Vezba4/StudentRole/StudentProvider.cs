using Common;
using RepositoryHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentRole
{
    public class StudentProvider : IStudents
    {
        
        public bool AddStudent(long jmbg, string name, string lastName)
        {
            try
            {
                StudentServer.tableHelper.AddOrReplaceStudent(new Student(jmbg, name, lastName));
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
