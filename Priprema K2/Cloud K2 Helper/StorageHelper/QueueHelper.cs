using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageHelper
{
    public class QueueHelper
    {
        #region Fields;
        CloudStorageAccount storageAccount;
        CloudQueueClient queueClient;
        CloudQueue queue;
        #endregion

        #region Kreiranje Reda
        //Kreira Red
        public QueueHelper(string queueName)
        {
            storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            queueClient = storageAccount.CreateCloudQueueClient();
            queue = queueClient.GetQueueReference(queueName);
            queue.CreateIfNotExists();
        }
        #endregion

        #region Dodavanje u red
        //Dodaje poruku u red
        public void AddToQueue(string messageContent)
        {
            CloudQueueMessage message = new CloudQueueMessage(messageContent);
            queue.AddMessage(message);
        }


        #endregion

        #region Skidanje sa reda
        //Skida poruku sa reda
        public string GetFromQueue()
        {
            CloudQueueMessage msg = queue.GetMessage();
            if (msg == null)
            {
                return null;
            }
            else
            {
                DeleteMessageFromQueue(msg);
                return  msg.AsString;
            }
        }


        #endregion

        #region Brisanje iz reda
        public void DeleteMessageFromQueue(CloudQueueMessage msg)
        {
            queue.DeleteMessage(msg);
        }
        #endregion
    }
}
