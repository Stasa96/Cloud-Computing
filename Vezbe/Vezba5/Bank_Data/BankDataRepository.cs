using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_Data
{
    public class BankDataRepository
    {
        private CloudStorageAccount _cloudStorageAccount;
        private CloudTable _table;
        private static bool _tableInitialised = false;
        TableOperation operation;

        public BankDataRepository()
        {
            _cloudStorageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("BankDataConnectionString"));
            CloudTableClient tableClient = new CloudTableClient(new Uri(_cloudStorageAccount.TableEndpoint.AbsoluteUri), _cloudStorageAccount.Credentials);

            _table = tableClient.GetTableReference("BankTable");
            _table.CreateIfNotExists();

            if (!_tableInitialised)
            {
                FillBank();
                _tableInitialised = true;
            }
        }

        private void FillBank()
        {
            /*BATCH OPERATION*/
            //Unatar batch-a pojedinacna obracanja tabeli neci biti prekidana drugim radnjama...
            TableBatchOperation batchOperation = new TableBatchOperation();

            User user1 = new User("001");
            user1.Balance = 5000;

            User user2 = new User("002");
            user2.Balance = 1000;

            User user3 = new User("003");
            user3.Balance = 1300;

            User user4 = new User("004");
            user4.Balance = 3000;
        
            User user5 = new User("005");
            user5.Balance = 6300;

            batchOperation.InsertOrReplace(user1);
            batchOperation.InsertOrReplace(user2);
            batchOperation.InsertOrReplace(user3);
            batchOperation.InsertOrReplace(user4);
            batchOperation.InsertOrReplace(user5);

            _table.ExecuteBatch(batchOperation);
        }

        public void AddUser(User newUser)
        {
            operation = TableOperation.Insert(newUser);
            _table.Execute(operation);
        }

        public void AddOrReplaceUser(User newUser)
        {
            operation = TableOperation.InsertOrReplace(newUser);
            _table.Execute(operation);
        }

        public void ReplaceUser(User user)
        {
            operation = TableOperation.Replace(user);
            _table.Execute(operation);
        }

        public void DeleteUser(User user)
        {
            operation = TableOperation.Delete(user);
            _table.Execute(operation);
        }

        public User RetriveSingleUser(string userID)
        {
            IQueryable<User> result = from g in _table.CreateQuery<User>()
                                      where g.PartitionKey.Equals("User") && g.RowKey.Equals(userID)
                                      select g;

            List<User> userResult = result.ToList<User>();
            if (userResult.Count<User>() == 0)
            {
                return null;
            }
            else
            {
                return userResult.First();
            }
        }

        public IQueryable<User> RetriveAllUsers()
        {
            IQueryable<User> result = from g in _table.CreateQuery<User>()
                                      where g.PartitionKey.Equals("User")
                                      select g;
            return result;
        }
    }
}
