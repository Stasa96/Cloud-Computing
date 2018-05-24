using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

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
    public  System.Drawing.Image DownloadImage(String containerName, String blobName)
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
    public string UploadImage(System.Drawing.Image image, String containerName, String blobName)
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
