using AzureService_data;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureService_data
{
    public class DataRepository
    {
        private CloudStorageAccount _storageAccount;
        private CloudTable _table;

        public DataRepository()
        {
            _storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            CloudTableClient tableClient = new CloudTableClient(new
           Uri(_storageAccount.TableEndpoint.AbsoluteUri), _storageAccount.Credentials);
            _table = tableClient.GetTableReference("RequestTable");
            _table.CreateIfNotExists();
        }

        public void AddRequest(RequestCountInfo request)
        {
            TableOperation add = TableOperation.InsertOrReplace(request);
            _table.Execute(add);
            
        }

        public List<RequestCountInfo> GetAllRequest()
        {
            IQueryable<RequestCountInfo> requests = from g in _table.CreateQuery<RequestCountInfo>()
                                                    where g.PartitionKey == "Request"
                                                    select g;
            return requests.ToList();
        }

        public RequestCountInfo GetOneRequest(string id)
        {
            IQueryable<RequestCountInfo> requests = from g in _table.CreateQuery<RequestCountInfo>()
                                                    where g.PartitionKey == "Request" && g.RowKey == id
                                                    select g;

            return requests.ToList()[0];
        }
    }
}
