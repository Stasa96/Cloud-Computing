using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsService_Data
{
    public class BlobHelper
    {
        CloudStorageAccount storageAccount =
       CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
        CloudBlobClient blobStorage;
        public BlobHelper()
        {
            blobStorage = storageAccount.CreateCloudBlobClient();
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
                image.Save(memoryStream, ImageFormat.Bmp);
                memoryStream.Position = 0;
                blob.Properties.ContentType = "image/bmp";
                blob.UploadFromStream(memoryStream);
                return blob.Uri.ToString();
            }
        }
    }
}
