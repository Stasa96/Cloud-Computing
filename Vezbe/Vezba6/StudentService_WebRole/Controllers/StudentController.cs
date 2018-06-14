using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using StudentsService_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentService_WebRole.Controllers
{
    public class StudentController : Controller
    {

        StudentDataRepository repo = new StudentDataRepository();
        
        // GET: Student
        public ActionResult Index()
        {
            return View(repo.RetrieveAllStudents());
        }

        public ActionResult Create()
        {
            return View("Create");
        }

        public ActionResult Edit(string id)
        {
            Student s = repo.RetrieveAllStudents().ToList().Find(st => st.RowKey.Equals(id));
            return View("Create",s);
        }

        public ActionResult Delete(string id)
        {
            Student s = repo.RetrieveAllStudents().ToList().Find(st => st.RowKey.Equals(id));

            repo.RemoveStudent(s);

            return View("Index", repo.RetrieveAllStudents());
        }

        public ActionResult Details(string id)
        {
            Student s = repo.RetrieveAllStudents().ToList().Find(st => st.RowKey.Equals(id));

            return View(s);
        }

        [HttpPost]
        public ActionResult AddEntity(String RowKey, String Name, String LastName,HttpPostedFileBase file )
        {
            try
            {
                // kreiranje blob sadrzaja i kreiranje blob klijenta
                string uniqueBlobName = string.Format("image_{0}", RowKey);
                var storageAccount =
                CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
                CloudBlobClient blobStorage = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobStorage.GetContainerReference("vezba");
                CloudBlockBlob blob = container.GetBlockBlobReference(uniqueBlobName);
                blob.Properties.ContentType = file.ContentType;
                // postavljanje odabrane datoteke (slike) u blob servis koristeci blob klijent
                blob.UploadFromStream(file.InputStream);
                // upis studenta u table storage koristeci StudentDataRepository klasu
                Student entry = new Student(RowKey)
                {
                    Name = Name,
                    LastName = LastName,
                    PhotoUrl = blob.Uri.ToString(),
                    ThumbnailUrl = blob.Uri.ToString()
                };
                repo.AddOrReplaceStudent(entry);
                return RedirectToAction("Index", repo.RetrieveAllStudents());
            }
            catch
            {
                return View("AddEntity");
            }
        }

    }
}