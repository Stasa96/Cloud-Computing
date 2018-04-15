using Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore
{
    public class BookstoreServerProvider : IBookstoreInteraction
    {
        static Dictionary<string, Book> books = new Dictionary<string, Book>();
        public double AddBook(string bookName, int count,double price)
        {
            Book pom;
            foreach (Book b in books.Values)
            {
                if(bookName == b.Name && price == 0)
                {
                    b.Count += count;
                    Trace.WriteLine($"{bookName} is added");
                    return count*b.Price;
                }
                else if(bookName == b.Name && price != 0)
                {
                    return -1;
                }
            }

            if (price != 0)
            {
                pom = new Book(bookName, count, price);
                books.Add(pom.Id, pom);
                return -2;
            }

            return -1;

        }

        public double GetPrice(string bookName)
        {
            foreach(Book b in books.Values)
            {
                if(b.Name == bookName)
                {
                    Trace.WriteLine($"Price is {b.Price}");
                    return b.Price;
                }
            }
            return -1;
        }

        public double removeBook(string bookName, int count)
        {
            foreach(Book b in books.Values)
            {
                if(bookName == b.Name && b.Count >= count)
                {
                    b.Count -= count;
                    Trace.WriteLine($"{bookName} is removed price:{b.Price}");
                    return count*b.Price;
                }
            }
            return -1;
        }
    }
}
