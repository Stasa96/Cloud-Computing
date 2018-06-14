using CloudServiceData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebRole.Controllers
{
    
    public class HomeController : Controller
    {
        TableHelper tableHelper = new TableHelper();
        BlobHelper blobHelper = new BlobHelper();
        QueueHelper queueHelper = new QueueHelper();

        public ActionResult Index()
        {
            return View(tableHelper.GetAllStudents());
        }

        [HttpPost]
        public ActionResult AddEntity(String RowKey, String Name, String LastName,HttpPostedFileBase file)
        {
            string url = blobHelper.UpladToBlob(blobHelper.GetReferenceOfBlob(RowKey, file),file);

            Student s = new Student(RowKey)
            {
                Name = Name,
                LastName = LastName,
                PhotoUrl = url,
                ThumbnailUrl = url

            };

            tableHelper.AddOrReplaceStudent(s);

            queueHelper.SendMessage(queueHelper.GetQueueReference("vezba7"),RowKey);

            return View("Index",tableHelper.GetAllStudents());
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}