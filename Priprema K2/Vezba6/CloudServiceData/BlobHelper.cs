using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CloudServiceData
{
    public class BlobHelper
    {
        public void InitBlobs()
        {
            try
            {
                // read account configuration settings
                var storageAccount =
                CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
                
                // create blob container for images
                CloudBlobClient blobStorage = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobStorage.GetContainerReference("kontejnerk2");
                container.CreateIfNotExists();
                // configure container for public access
                var permissions = container.GetPermissions();
                permissions.PublicAccess = BlobContainerPublicAccessType.Container;
                container.SetPermissions(permissions);
            }
            catch (WebException)
            {
            }
        }
        public CloudBlockBlob GetReferenceOfBlob(string RowKey, HttpPostedFileBase file) {
            // kreiranje blob sadrzaja i kreiranje blob klijenta
            string uniqueBlobName = string.Format("image_{0}", RowKey);
            var storageAccount =
            CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
    
            CloudBlobClient blobStorage = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobStorage.GetContainerReference("kontejnerk2");
            CloudBlockBlob blob = container.GetBlockBlobReference(uniqueBlobName);
            blob.Properties.ContentType = file.ContentType;

            return blob;
        }

        public string UpladToBlob(CloudBlockBlob blob, HttpPostedFileBase file)
        {
            blob.UploadFromStream(file.InputStream);

            return blob.Uri.ToString();
        }
    }
}
