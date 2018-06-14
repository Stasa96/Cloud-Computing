using CloudServiceData;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudServiceData
{
    public class TableHelper
    {
        private CloudStorageAccount _storageAccount;
        private CloudTable _table;

        public TableHelper()
        {
            _storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            CloudTableClient tableClient = new CloudTableClient(new Uri(_storageAccount.TableEndpoint.AbsoluteUri),
                                                                _storageAccount.Credentials);
            _table = tableClient.GetTableReference("StudentTableK2");
            if (_table.CreateIfNotExists())
            { 
                //InitStudents();
            }
        }

        //private void InitStudents()
        //{
        //    TableBatchOperation batchOperation = new TableBatchOperation();

        //    Student a1 = new Student(1,"Ime1","Prezime1");
        //    Student a2 = new Student(2, "Ime2", "Prezime2");
        //    Student a3 = new Student(3, "Ime3", "Prezime3");
        //    Student a4 = new Student(4, "Ime4", "Prezime4");
        //    Student a5 = new Student(5, "Ime5", "Prezime5");

        //    batchOperation.InsertOrReplace(a1);
        //    batchOperation.InsertOrReplace(a2);
        //    batchOperation.InsertOrReplace(a3);
        //    batchOperation.InsertOrReplace(a4);
        //    batchOperation.InsertOrReplace(a5);

        //    _table.ExecuteBatch(batchOperation);
        //}

        public void AddOrReplaceStudent(Student student)
        {
            TableOperation add = TableOperation.InsertOrReplace(student);
            _table.Execute(add);

        }

        public void DeleteStudent(Student student)
        {
            TableOperation delete = TableOperation.Delete(student);
            _table.Execute(delete);
        }

        public List<Student> GetAllStudents()
        {
            IQueryable<Student> requests = from g in _table.CreateQuery<Student>()
                                                 where g.PartitionKey == "Student"
                                                 select g;
            return requests.ToList();
        }

        public Student GetOneStudent(string id)
        {
            IQueryable<Student> requests = from g in _table.CreateQuery<Student>()
                                                 where g.PartitionKey == "Student" && g.RowKey == id
                                                 select g;

            return requests.ToList()[0];
        }
    }
}
