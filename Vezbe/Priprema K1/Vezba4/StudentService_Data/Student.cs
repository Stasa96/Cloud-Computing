using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentService_Data
{
    public class Student :TableEntity
    {
        string firstName;
        string lastName;

        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }

        public Student(string indeks)
        {
            PartitionKey = "Student";
            RowKey = indeks;
        }

        public Student()
        {

        }
    }
}
