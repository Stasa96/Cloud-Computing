using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore_Data
{
    class BookstoreDataRepository
    {
        CloudTable _table;
        CloudStorageAccount _cloudStorageAccount;
        static bool _tableInitialised = false;

        public BookstoreDataRepository()
        {
            _cloudStorageAccount = CloudStorageAccount.Parse
        }
    }
}
