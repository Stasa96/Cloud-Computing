using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryHelper
{
    public class Student :TableEntity
    {
        Int64 jmbg;
        string name;
        string lastName;

        public Student() { }
        public Student(long jmbg, string name, string lastName)
        {
            PartitionKey = "Student";
            RowKey = jmbg.ToString();
            this.jmbg = jmbg;
            this.name = name;
            this.lastName = lastName;
        }

        public long Jmbg { get => jmbg; set => jmbg = value; }
        public string Name { get => name; set => name = value; }
        public string LastName { get => lastName; set => lastName = value; }
    }
}
