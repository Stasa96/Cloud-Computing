using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudServiceData
{
    public class QueueHelper
    {
        public CloudQueue GetQueueReference(String queueName)
        {
            CloudStorageAccount storageAccount =
           CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference(queueName);
            queue.CreateIfNotExists();
            return queue;
        }        public void SendMessage(CloudQueue queue, string msg)
        {
            queue.AddMessage(new CloudQueueMessage(msg));
        }        public string GetMessage(CloudQueue queue)
        {
            CloudQueueMessage m = queue.GetMessage();
            if (m == null)
            {
                return null;
            }
            else
            {


                return  m.AsString;
            }
        }        
    }
}
