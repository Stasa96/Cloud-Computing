using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudService_Data
{
    public class TableHelper
    {
        private CloudTable table;
        private static Int32 evidenceNumber = 0;

        public TableHelper()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            CloudTableClient tableClient = new CloudTableClient(new Uri(storageAccount.TableEndpoint.AbsoluteUri), storageAccount.Credentials);
            table = tableClient.GetTableReference("EvidenceTable");
            table.CreateIfNotExists();

            evidenceNumber = RetrieveAllEvidence().ToList().Count();
        }

        public static Int32 GetNextRowkey()
        {
            return evidenceNumber++;
        }

        public EvidenceEntity RetriveSingleEvidence(String rowKey)
        {
            try
            {
                var results = from g in table.CreateQuery<EvidenceEntity>()
                              where g.PartitionKey.Equals("Evidence") && g.RowKey.Equals(rowKey)
                              select g;
                return results.First();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<EvidenceEntity> RetrieveAllEvidence()
        {
            try
            {
                var results = from g in table.CreateQuery<EvidenceEntity>()
                              where g.PartitionKey == "Evidence"
                              select g;
                return results.ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool AddEvidence(EvidenceEntity entity)
        {
            if (Exists(entity.RowKey))
            {
                return false;
            }

            try
            {
                TableOperation insertOperation = TableOperation.Insert(entity);
                table.Execute(insertOperation);
                return true;
            }
            catch (Exception e)
            {
                Trace.TraceError($"ERROR: {e.Message}");
                return false;
            }
        }

        public bool DeleteEntity(String rowKey)
        {
            if (!Exists(rowKey))
            {
                return false;
            }

            try
            {
                EvidenceEntity entity = RetriveSingleEvidence(rowKey);
                TableOperation deleteOperation = TableOperation.Delete(entity);
                table.Execute(deleteOperation);
                return true;
            }
            catch (Exception e)
            {
                Trace.TraceError($"ERROR: {e.Message}");
                return false;
            }

        }

        public bool ReplaceEntity(EvidenceEntity entity)
        {
            if (!Exists(entity.RowKey))
            {
                return false;
            }

            try
            {
                TableOperation replaceOperation = TableOperation.Replace(entity);
                table.Execute(replaceOperation);
                return true;
            }
            catch (Exception e)
            {
                Trace.TraceError($"ERROR: {e.Message}");
                return false;
            }
        }

        public bool Exists(String indexNo)
        {
            bool exists = false;
            TableResult tableResult = null;

            try
            {
                TableOperation retrive = TableOperation.Retrieve("Evidence", indexNo);
                tableResult = table.Execute(retrive);
            }
            catch (Exception e)
            {
                Trace.TraceError($"ERROR: {e.Message}");
                return false;
            }


            if (tableResult.Result != null)
            {
                exists = true;
            }

            return exists;
        }
    }
}
