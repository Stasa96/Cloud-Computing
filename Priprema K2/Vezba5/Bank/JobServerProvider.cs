using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    public class JobServerProvider : IBank
    {
        string prepUserID = null;
        double prepAmnount = 0;
        public void Commit()
        {
            if (prepUserID == null)
            {
                return;
            }

            CloudServiceData.Bank prepUser = JobServer.tableHelper.GetOneBank(prepUserID + "prep");

            if (prepUser != null)
            {
                JobServer.tableHelper.DeleteBank(prepUser);

                string updateUserID = prepUser.RowKey.Remove(prepUser.RowKey.IndexOf("prep"));
                CloudServiceData.Bank updateUser = JobServer.tableHelper.GetOneBank(updateUserID);

                updateUser.Amount = prepUser.Amount;

                JobServer.tableHelper.AddOrReplaceBank(updateUser);
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
            string retval = "";
            JobServer.tableHelper.GetAllBanks().ToList().ForEach(u =>
            {
                retval += u.ToString() + "\n----------------------\n";
            });

            return retval;
        }

        public bool Prepare()
        {
            if (prepUserID == null)
            {
                return false;
            }

            CloudServiceData.Bank user = JobServer.tableHelper.GetOneBank(prepUserID);

            if (user != null && user.Amount - prepAmnount >= 0)
            {
                CloudServiceData.Bank prepUser = new CloudServiceData.Bank(user.RowKey + "prep")
                {
                    Amount = user.Amount - prepAmnount
                };

                JobServer.tableHelper.AddOrReplaceBank(prepUser);

                return true;
            }
            prepUserID = null;
            prepAmnount = 0;
            return false;
        }

        public void Rollback()
        {
            if (prepUserID == null)
            {
                return;
            }

            CloudServiceData.Bank prepUser = JobServer.tableHelper.GetOneBank(prepUserID + "prep");

            if (prepUser != null)
            {
                JobServer.tableHelper.DeleteBank(prepUser);
            }

            prepUserID = null;
            prepAmnount = 0;
        }
    }
}
