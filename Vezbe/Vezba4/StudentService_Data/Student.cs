using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentService_Data
{
    public class Student : TableEntity
    {
        public String Name { get; set; }

        public string Index { get; set; }

        public String LastName { get; set; }
        public Student(String index)
        {
            PartitionKey = "Student";
            RowKey = index;
            Index = index;
        }
        public Student() { }
    }
}
