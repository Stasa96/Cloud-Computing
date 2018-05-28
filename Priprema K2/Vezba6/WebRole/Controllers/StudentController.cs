using CloudServiceData;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebRole.Controllers
{
    public class StudentController : Controller
    {
        TableHelper tableHelper = new TableHelper();
        BlobHelper blobHelper = new BlobHelper();
        // GET: Strudent
        public ActionResult Index()
        {
            return View(tableHelper.GetAllStudents());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddEntity(String RowKey, String Name, String LastName,HttpPostedFileBase file)
        {
            CloudBlockBlob blob  = blobHelper.GetReferenceOfBlob(RowKey,file);
            blobHelper.InitBlobs();

            string reff = blobHelper.UpladToBlob(blob, file);
            Student s = new Student(RowKey)
            {
                Name = Name,
                LastName = LastName,
                PhotoUrl = reff,
                ThumbnailUrl = reff
            };
            tableHelper.AddOrReplaceStudent(s);

            return View("index", tableHelper.GetAllStudents());
        }

        public ActionResult Delete(string id)
        {
            tableHelper.DeleteStudent(tableHelper.GetOneStudent(id));
            return View("Index", tableHelper.GetAllStudents());
        }

        public ActionResult Details(string id)
        {
            return View("Index", new List<Student>() { tableHelper.GetOneStudent(id) });
        }

        public ActionResult Edit(string id)
        {
            return View("Create", tableHelper.GetOneStudent(id));
        }
    }
}