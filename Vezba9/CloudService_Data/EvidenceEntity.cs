using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudService_Data
{
    public class EvidenceEntity : TableEntity
    {
        public String Evidence { get; set; }
        public String StackTrace { get; set; }

        public EvidenceEntity(String evidence)
        {
            PartitionKey = "Evidence";
            RowKey = (TableHelper.GetNextRowkey()).ToString();
            Evidence = evidence;
        }

        public EvidenceEntity(String evidence, String stackTrace)
        {
            PartitionKey = "Evidence";
            RowKey = (TableHelper.GetNextRowkey()).ToString();
            Evidence = evidence;
            StackTrace = stackTrace;
        }

        public EvidenceEntity() { }
    }
}
