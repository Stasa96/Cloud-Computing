using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore_Data
{
    public class BookstoreDataRepository
    {
        private CloudStorageAccount _cloudStorageAccount;
        private CloudTable _table;
        private static bool _tableInitialised = false;
        TableOperation operation;

        public BookstoreDataRepository()
        {
            Trace.WriteLine("USAOOOOOOOOOOOOOOOOOOOO1");
            _cloudStorageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("BookstoreDataConnectionString"));
            CloudTableClient tableClient = new CloudTableClient(new Uri(_cloudStorageAccount.TableEndpoint.AbsoluteUri), _cloudStorageAccount.Credentials);

            _table = tableClient.GetTableReference("BookstoreTable");
            _table.CreateIfNotExists();

            if (!_tableInitialised)
            {
                FillBookstore();
                _tableInitialised = true;
            }
        }

        private void FillBookstore()
        {
            TableBatchOperation operations = new TableBatchOperation();

            Book b1 = new Book("001");
            b1.Name = "Na drini cuprija";
            b1.Price = new Random().Next(500,1000);
            b1.Count = new Random().Next(10);

            Book b2 = new Book("002");
            b2.Name = "Besnilo";
            b2.Price = new Random().Next(500, 1000);
            b2.Count = new Random().Next(10);

            Book b3 = new Book("003");
            b3.Name = "Lovac u zitu";
            b3.Price = new Random().Next(500, 1000);
            b3.Count = new Random().Next(10);

            Book b4 = new Book("004");
            b4.Name = "Alhemicar";
            b4.Price = new Random().Next(500, 1000);
            b4.Count = new Random().Next(10);

            Book b5 = new Book("005");
            b5.Name = "Ja,Aleks Kros";
            b5.Price = new Random().Next(500, 1000);
            b5.Count = new Random().Next(10);

            operations.InsertOrReplace(b1);
            operations.InsertOrReplace(b2);
            operations.InsertOrReplace(b3);
            operations.InsertOrReplace(b4);
            operations.InsertOrReplace(b5);

            _table.ExecuteBatch(operations);
        }

        public void AddBook(Book b)
        {
            operation = TableOperation.Insert(b);
            _table.Execute(operation);
        }

        public void RemoveBook(Book b)
        {
            operation = TableOperation.Delete(b);
            _table.Execute(operation);
        }

        public void AddOrReplaceBook(Book b)
        {
            operation = TableOperation.InsertOrReplace(b);
            _table.Execute(operation);
        }

        public void ReplaceBook(Book b)
        {
            operation = TableOperation.Replace(b);
            _table.Execute(operation);
        }

        public IQueryable<Book> RetrieveAllBooks()
        {
            return from g in _table.CreateQuery<Book>()
                   where g.PartitionKey == "Book"
                   select g;
        }


        public Book RetrieveOneBook(string bookID)
        {
            IQueryable<Book> ret =  from g in _table.CreateQuery<Book>()
                                    where g.PartitionKey == "Book" && g.RowKey== bookID
                                    select g;

            try
            {
                return ret.ToList()[0];
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}
