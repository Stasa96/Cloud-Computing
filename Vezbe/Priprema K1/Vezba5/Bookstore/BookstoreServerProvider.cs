using Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore
{
    public class BookstoreServerProvider : IBookstore
    {
        static Dictionary<string, Book> books = new Dictionary<string, Book>() { { "123", new Book("123", "Ime", 700, 5) }, { "1235", new Book("1235", "Ime1", 700, 5) }, { "1263", new Book("1263", "Im3e", 700, 5) } };
        public void EnlistPurchase(string bookID, uint count)
        {
            Book b;

            if(books.TryGetValue(bookID,out b))
            {
                if(b.Count >= count)
                {
                    Trace.WriteLine($"{b.Name} is enlisted, count: {count}");
                    books[bookID].Count -= count;
                    ListAvailableItems();
                }
                else
                {
                    Trace.WriteLine($"{b.Name} doesn't have {count} copies.");
                }
            }
            else
            {
                Trace.WriteLine($"{bookID} doesn't exists in bookstore");
            }
        }

        public double GetItemPrice(string bookID)
        {
            Book b;

            if(books.TryGetValue(bookID,out b))
            {
                return b.Price;
            }
            return -1;
        }

        public void ListAvailableItems()
        {
            foreach(Book b in books.Values)
            {
                Trace.WriteLine("-----------------------");
                Trace.WriteLine($"ID: {b.Id}\nName: {b.Name}\nPrice: {b.Price}\nCount: {b.Count}");
                Trace.WriteLine("-----------------------");
            }
        }
    }
}
