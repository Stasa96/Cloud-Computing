using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_Data
{
    public class User : TableEntity
    {
        double balance;

        public User(string userID)
        {
            PartitionKey = "User";
            RowKey = userID;
        }

        public User() { }

        public override string ToString()
        {
            return $"ID: {RowKey}\tBalance: {balance}";
        }

        public double Balance { get => balance; set => balance = value; }
    }
}
