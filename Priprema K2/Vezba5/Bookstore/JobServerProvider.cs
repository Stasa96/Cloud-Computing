using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore
{
    public class JobServerProvider : IBookstore
    {
        string prepBookID = null;
        int prepCount;


        public void EnlistPurchase(string bookID, uint count)
        {
            prepBookID = bookID;
            prepCount = (int)count;
        }

        public double GetItemPrice(string bookID)
        {
            CloudServiceData.Bookstore book = JobServer.tableHelper.GetOneBookstore(bookID);

            if (book != null)
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

            JobServer.tableHelper.GetAllBookstores().ToList().ForEach(book =>
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

            CloudServiceData.Bookstore book = JobServer.tableHelper.GetOneBookstore(prepBookID + "prep");

            if (book != null)
            {

                JobServer.tableHelper.DeleteBookstore(book);
                string updateBookID = book.RowKey.Remove(book.RowKey.IndexOf("prep"));
                CloudServiceData.Bookstore updateBook = JobServer.tableHelper.GetOneBookstore(updateBookID);

                updateBook.Cnt = book.Cnt;

                JobServer.tableHelper.AddOrReplaceBookstore(updateBook);
            }
        }
        public bool Prepare()
        {
            if (prepBookID == null)
            {
                return false;
            }

            CloudServiceData.Bookstore book = JobServer.tableHelper.GetOneBookstore(prepBookID);

            if (book != null && book.Cnt - prepCount >= 0)
            {
                CloudServiceData.Bookstore prepBook = new CloudServiceData.Bookstore(prepBookID + "prep")
                {
                    Price = book.Price,
                    Cnt = book.Cnt - prepCount

                };

                JobServer.tableHelper.AddOrReplaceBookstore(prepBook);

                return true;
            }
            prepBookID = null;
            prepCount = 0;
            return false;
        }

        public void Rollback()
        {
            if (prepBookID == null)
            {
                return;
            }

            CloudServiceData.Bookstore book = JobServer.tableHelper.GetOneBookstore(prepBookID + "prep");

            if (book != null)
            {
                JobServer.tableHelper.DeleteBookstore(book);
            }

            prepBookID = null;
            prepCount = 0;
        }
    }
}
