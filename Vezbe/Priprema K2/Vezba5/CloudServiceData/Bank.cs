using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudServiceData
{
    public class Bank : TableEntity
    {
        string userID;
        double amount;

        public Bank() { }

        public Bank(string userID)
        {
            this.userID = userID;
            PartitionKey = "Bank";
            RowKey = userID;
        }

        public Bank(string userID, double amount)
        {
            PartitionKey = "Bank";
            RowKey = userID;
            this.userID = userID;
            this.amount = amount;
        }

        public string UserID { get => userID; set => userID = value; }
        public double Amount { get => amount; set => amount = value; }

        public override string ToString()
        {
            return $"{UserID}, {userID}";
        }
    }
}
