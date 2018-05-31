using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CloudServiceData
{
    public class BlobHelper
    {
        CloudBlobClient blobStorage;

        public BlobHelper()
        {
            InitBlobs();
        }
        public void InitBlobs()
        {
            try
            {
                // read account configuration settings
                var storageAccount =
                CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
                
                // create blob container for images
                blobStorage = storageAccount.CreateCloudBlobClient();
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
    
            blobStorage = storageAccount.CreateCloudBlobClient();
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

        public Image DownloadImage(String containerName, String blobName)
        {
            CloudBlobContainer container =
           blobStorage.GetContainerReference(containerName);
            CloudBlockBlob blob = container.GetBlockBlobReference(blobName);
            using (MemoryStream ms = new MemoryStream())
            {
                blob.DownloadToStream(ms);
                return new Bitmap(ms);
            }
        }
        public string UploadImage(Image image, String containerName, String blobName)
        {
            CloudBlobContainer container =
           blobStorage.GetContainerReference(containerName);
            CloudBlockBlob blob = container.GetBlockBlobReference(blobName);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Bmp);
                memoryStream.Position = 0;
                blob.Properties.ContentType = "image/bmp";
                blob.UploadFromStream(memoryStream);
                return blob.Uri.ToString();
            }
        }
    }
}
