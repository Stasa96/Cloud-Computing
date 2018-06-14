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
        private CloudTable _tableBank;
        private CloudTable _tableBookstore;
        CloudTableClient tableClient;

        public TableHelper()
        {
            _storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            tableClient = new CloudTableClient(new Uri(_storageAccount.TableEndpoint.AbsoluteUri),
                                                                _storageAccount.Credentials);
            
        }

        public  void Bank()
        {
            _tableBank = tableClient.GetTableReference("BankK2");
            if (_tableBank.CreateIfNotExists())
            {
                InitBank();
            }
        }

        public  void Bookstore()
        {
            _tableBookstore = tableClient.GetTableReference("BookstoreK2");
            if (_tableBookstore.CreateIfNotExists())
            {
                InitBookstore();
            }
        }

        private void InitBank()
        {
            TableBatchOperation batchOperation = new TableBatchOperation();

            Bank a1 = new Bank("1",20000);
            Bank a2 = new Bank("2", 1600);
            Bank a3 = new Bank("3", 6000);
            Bank a4 = new Bank("4", 9000);
            Bank a5 = new Bank("5",5000);

            batchOperation.InsertOrReplace(a1);
            batchOperation.InsertOrReplace(a2);
            batchOperation.InsertOrReplace(a3);
            batchOperation.InsertOrReplace(a4);
            batchOperation.InsertOrReplace(a5);

            _tableBank.ExecuteBatch(batchOperation);
        }

        private void InitBookstore()
        {
            TableBatchOperation batchOperation = new TableBatchOperation();

            Bookstore a1 = new Bookstore("1",5, 800);
            Bookstore a2 = new Bookstore("2", 5, 900);
            Bookstore a3 = new Bookstore("3", 5, 1000);
            Bookstore a4 = new Bookstore("4", 5, 1100);
            Bookstore a5 = new Bookstore("5", 5, 1500);

            batchOperation.InsertOrReplace(a1);
            batchOperation.InsertOrReplace(a2);
            batchOperation.InsertOrReplace(a3);
            batchOperation.InsertOrReplace(a4);
            batchOperation.InsertOrReplace(a5);

            _tableBookstore.ExecuteBatch(batchOperation);
        }
        //bank
        public void AddOrReplaceBank(Bank Bank)
        {
            TableOperation add = TableOperation.InsertOrReplace(Bank);
            _tableBank.Execute(add);

        }

        public void DeleteBank(Bank Bank)
        {
            TableOperation delete = TableOperation.Delete(Bank);
            _tableBank.Execute(delete);
        }
        public List<Bank> GetAllBanks()
        {
            IQueryable<Bank> requests = from g in _tableBank.CreateQuery<Bank>()
                                           where g.PartitionKey == "Bank"
                                           select g;
            return requests.ToList();
        }

        public Bank GetOneBank(string id)
        {
            IQueryable<Bank> requests = from g in _tableBank.CreateQuery<Bank>()
                                           where g.PartitionKey == "Bank" && g.RowKey == id
                                           select g;

            return requests.ToList()[0];
        }

        //bookstore
        public void AddOrReplaceBookstore(Bookstore Bookstore)
        {
            TableOperation add = TableOperation.InsertOrReplace(Bookstore);
            _tableBookstore.Execute(add);

        }

        public void DeleteBookstore(Bookstore Bookstore)
        {
            TableOperation delete = TableOperation.Delete(Bookstore);
            _tableBookstore.Execute(delete);
        }

        public List<Bookstore> GetAllBookstores()
        {
            IQueryable<Bookstore> requests = from g in _tableBookstore.CreateQuery<Bookstore>()
                                           where g.PartitionKey == "Bookstore"
                                           select g;
            return requests.ToList();
        }

        public Bookstore GetOneBookstore(string id)
        {
            IQueryable<Bookstore> requests = from g in _tableBookstore.CreateQuery<Bookstore>()
                                           where g.PartitionKey == "Bookstore" && g.RowKey == id
                                           select g;

            return requests.ToList()[0];
        }
    }
}
