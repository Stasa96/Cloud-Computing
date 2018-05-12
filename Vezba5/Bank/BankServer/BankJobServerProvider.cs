using Bank_Data;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.BankServer
{
    public class BankJobServerProvider : IBank
    {
        BankDataRepository repository = new BankDataRepository();
        string prepUserID = null;
        double prepAmnount = 0;

        public void Commit()
        {
            if(prepUserID == null)
            {
                return;
            }

            User prepUser = repository.RetriveSingleUser(prepUserID+"prep");

            if(prepUser != null)
            {
                repository.DeleteUser(prepUser);

                string updateUserID = prepUser.RowKey.Remove(prepUser.RowKey.IndexOf("prep"));
                User updateUser = repository.RetriveSingleUser(updateUserID);

                updateUser.Balance = prepUser.Balance;

                repository.ReplaceUser(updateUser);
            }

            prepUserID = null;
            prepAmnount = 0;
        }

        public void EnlistMoneyTransfer(string userID, double amount)
        {
            prepAmnount = amount;
            prepUserID = userID;
        }

        public string ListClients()
        {
            string retval ="";
            repository.RetriveAllUsers().ToList().ForEach(u => 
            {
                retval += u.ToString() + "\n----------------------\n";
            });

            return retval;
            
        }

        public bool Prepare()
        {  
            if(prepUserID == null)
            {
                return false;
            }

            User user = repository.RetriveSingleUser(prepUserID);

            if(user!= null && user.Balance - prepAmnount >= 0)
            {
                User prepUser = new User(user.RowKey + "prep")
                {
                    Balance = user.Balance - prepAmnount
                };

                repository.AddUser(prepUser);

                return true;
            }

            return false;
        }

        public void Rollback()
        {
            if(prepUserID == null)
            {
                return;
            }

            User prepUser = repository.RetriveSingleUser(prepUserID+"prep");

            if(prepUser != null)
            {
                repository.DeleteUser(prepUser);
            }

            prepUserID = null;
            prepAmnount = 0;
        }
    }
}
