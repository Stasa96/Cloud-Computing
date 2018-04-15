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
        int _availableNo;
        double _price;

        public int AvailableNo { get => _availableNo; set => _availableNo = value; }
        public double Price { get => _price; set => _price = value; }

        public Book(string bookID)
        {
            PartitionKey = "Book";
            RowKey = bookID;
        }

        public Book()
        {

        }
    }
}
