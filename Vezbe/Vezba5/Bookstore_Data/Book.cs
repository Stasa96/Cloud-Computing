using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore_Data
{
    public class Book : TableEntity
    {
        public Book() { }

        int count;
        double price;
        string name;

        public double Price { get => price; set => price = value; }
        public string Name { get => name; set => name = value; }
        public int Count { get => count; set => count = value; }

        public Book(string bookID)
        {
            PartitionKey = "Book";
            RowKey = bookID;
        }

        public override string ToString()
        {
            return $"ID: {RowKey}\nName: {name}\nPrice: {price}\nCount: {count}\n";
        }
    }
}
