using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsService_Data
{
    public class QueueHelper
    {
        public static CloudQueue GetQueueReference(String queueName)
        {
            CloudStorageAccount storageAccount =
            CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference(queueName);
            queue.CreateIfNotExists();
            return queue;
        }
		
		public void AddToQueue(string RowKey)
        {
            CloudQueue queue = QueueHelper.GetQueueReference("vezba");
            CloudQueueMessage message = new CloudQueueMessage(RowKey);
            queue.AddMessage(message);
        }
    }
}
