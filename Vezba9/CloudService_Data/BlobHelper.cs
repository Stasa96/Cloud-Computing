using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CloudService_Data
{
    public class BlobHelper
    {
        static CloudBlobContainer container;

        public BlobHelper()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            container = blobClient.GetContainerReference("servicehoststatus");
            container.CreateIfNotExists();

            BlobContainerPermissions permissions = container.GetPermissions();
            permissions.PublicAccess = BlobContainerPublicAccessType.Container;
            container.SetPermissions(permissions);
        }

        public CloudBlockBlob UploadToBlob(String uniqueBlobName, String status)
        {
            CloudBlockBlob blob = null;

            using (var stream = new MemoryStream(Encoding.Default.GetBytes(status)))
            {
                try
                {
                    blob = container.GetBlockBlobReference(uniqueBlobName);
                    blob.UploadFromStream(stream);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return blob;
        }

        public String DownloadFromBlob(String uniqueBlobName)
        {
            String text = "";

            using (var stream = new MemoryStream())
            {
                try
                {
                    CloudBlockBlob blob = container.GetBlockBlobReference(uniqueBlobName);
                    blob.DownloadToStream(stream);
                    text = Encoding.Default.GetString(stream.ToArray());
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return text;
        }

        public static void InitBlobs()
        {
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));

                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                container = blobClient.GetContainerReference("servicehoststatus");
                container.CreateIfNotExists();

                BlobContainerPermissions permissions = container.GetPermissions();
                permissions.PublicAccess = BlobContainerPublicAccessType.Container;
                container.SetPermissions(permissions);
            }
            catch (WebException e)
            {
                Trace.TraceError($"ERROR: {e.Message}");
            }
        }
    }
}
