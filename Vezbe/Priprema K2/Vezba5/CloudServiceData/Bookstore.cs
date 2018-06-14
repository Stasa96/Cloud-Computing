using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudServiceData
{
    public class Bookstore : TableEntity
    {
        string bookID;
        int cnt;
        double price;

        public Bookstore(string bookID, int cnt, double price)
        {
            PartitionKey = "Bookstore";
            RowKey = bookID;
            this.bookID = bookID;
            this.cnt = cnt;
            this.price = price;
        }

        public Bookstore() { }

        public Bookstore(string bookID)
        {
            this.bookID = bookID;
            PartitionKey = "Bookstore";
            RowKey = bookID;
        }

        public string BookID { get => bookID; set => bookID = value; }
        public int Cnt { get => cnt; set => cnt = value; }
        public double Price { get => price; set => price = value; }

        public override string ToString()
        {
            return $"{BookID}, {Cnt}, {price}";
        }
    }
}
