using Bookstore_Data;
using Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.BookstoreServer
{
    public class BookstoreJobServerProvider : IBookstore
    {
        BookstoreDataRepository repository = new BookstoreDataRepository();
        string prepBookID = null;
        int prepCount;

        public BookstoreJobServerProvider()
        {
            repository = new BookstoreDataRepository();
            Trace.WriteLine("USAOOOOOOOOOOOOOOOOOOOO");
        }

        public void EnlistPurchase(string bookID, uint count)
        {
            prepBookID = bookID;
            prepCount = (int)count;
        }

        public double GetItemPrice(string bookID)
        {
            Book book = repository.RetrieveOneBook(bookID);

            if(book != null)
            {
                return book.Price;
            }
            else
            {
                return -1;
            }
        }

        public string ListAvailableItems()
        {
            string retVal = "";

            repository.RetrieveAllBooks().ToList().ForEach(book =>
            {
                retVal += book.ToString() + "\n----------------------\n";
            });

            return retVal;
        }

        public void Commit()
        {
            if (prepBookID == null)
            {
                return;
            }

            Book book = repository.RetrieveOneBook(prepBookID+"prep");

            if (book != null)
            {

                repository.RemoveBook(book);
                string updateBookID = book.RowKey.Remove(book.RowKey.IndexOf("prep"));
                Book updateBook = repository.RetrieveOneBook(updateBookID);

                updateBook.Count = book.Count;

                repository.ReplaceBook(updateBook);
            }
        }
        public bool Prepare()
        {
            if(prepBookID == null)
            {
                return false;
            }

            Book book = repository.RetrieveOneBook(prepBookID);

            if(book != null && book.Count - prepCount >=0)
            {
                Book prepBook = new Book(prepBookID + "prep")
                {
                    Price = book.Price,
                    Count = book.Count - prepCount
                    
                };

                repository.AddBook(prepBook);

                return true;
            }

            return false;
        }

        public void Rollback()
        {
           if(prepBookID == null)
           {
                return;
           }

            Book book = repository.RetrieveOneBook(prepBookID + "prep");
            
            if(book != null)
            {
                repository.RemoveBook(book);
            }

            prepBookID = null;
            prepCount = 0;
        }
    }
}
