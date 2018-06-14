using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentService_Data
{
    public class StudentDataRepository
    {
        CloudTable cloudTable;
        CloudStorageAccount cloudStorageAccount;
    }
}
