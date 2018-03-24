using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Linq;


namespace StudentService_Data
{
    public class StudentDataRepository
    {
        private CloudStorageAccount _storageAccount;
        private CloudTable _table;
        

        public StudentDataRepository()
        {
            _storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            CloudTableClient tableClient = new CloudTableClient(new
           Uri(_storageAccount.TableEndpoint.AbsoluteUri), _storageAccount.Credentials);
            _table = tableClient.GetTableReference("StudentTable");
            _table.CreateIfNotExists();
        }
        public IQueryable<Student> RetrieveAllStudents()
        {
            var results = from g in _table.CreateQuery<Student>()
                          where g.PartitionKey == "Student"
                          select g;
            return results;
        }
        public void AddStudent(Student newStudent)
        {
            TableOperation insertOperation = TableOperation.Insert(newStudent);
            _table.Execute(insertOperation);

        }

        public void RemoveStudent(Student newStudent)
        {
            TableOperation deleteOperation = TableOperation.Delete(newStudent);
            _table.Execute(deleteOperation);
        }

        public void UpdateStudent(Student newStudent)
        {
            newStudent.ETag = "*"; // ZASTO SAMO OVDE
            TableOperation replaceOperation = TableOperation.Replace(newStudent);
            _table.Execute(replaceOperation); 
        }
    }
}
