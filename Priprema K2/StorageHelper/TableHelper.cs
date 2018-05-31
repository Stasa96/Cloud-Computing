using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageHelper
{
    public class TableHelper
    {
        #region Fields
        CloudStorageAccount storageAccount;
        CloudTable table;
        CloudTableClient tableClient;
        #endregion

        #region Kreiranje tabele
        // Kreiranje tabele
        public TableHelper(string tableName)
        {
            storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            tableClient = new CloudTableClient(new Uri(storageAccount.TableEndpoint.AbsoluteUri),
                                                                storageAccount.Credentials);
            table = tableClient.GetTableReference(tableName);
            if (table.CreateIfNotExists())
            {
                InitTable();
            }
         }
        #endregion

        private void InitTable()
        {
            TableBatchOperation tableOperations = new TableBatchOperation();

            Proizvod a1 = new Proizvod("123",10);
            Proizvod a2 = new Proizvod("456", 10);
            Proizvod a3 = new Proizvod("789", 10);
            Proizvod a4 = new Proizvod("000", 10);

            tableOperations.InsertOrReplace(a1);
            tableOperations.InsertOrReplace(a2);
            tableOperations.InsertOrReplace(a3);
            tableOperations.InsertOrReplace(a4);

            table.ExecuteBatch(tableOperations);
        }

        #region Operacije nad tabelom
        //Operacije nad tabelom
        //Find: Proizvod -> Replace: Naziv klase koja se koristi
        public void AddOrReplaceProizvod(Proizvod obj)
        {
            TableOperation add = TableOperation.InsertOrReplace(obj);
            table.Execute(add);

        }

        public void DeleteProizvod(Proizvod obj)
        {
            TableOperation delete = TableOperation.Delete(obj);
            table.Execute(delete);
        }

        public List<Proizvod> GetAllProizvods()
        {
            IQueryable<Proizvod> requests = from g in table.CreateQuery<Proizvod>()
                                           where g.PartitionKey == "Proizvod"
                                           select g;
            return requests.ToList();
        }

        public Proizvod GetOneProizvod(string id)
        {
            IQueryable<Proizvod> requests = from g in table.CreateQuery<Proizvod>()
                                           where g.PartitionKey == "Proizvod" && g.RowKey == id
                                           select g;

            return requests.ToList()[0];
        }
        #endregion
    }
}
