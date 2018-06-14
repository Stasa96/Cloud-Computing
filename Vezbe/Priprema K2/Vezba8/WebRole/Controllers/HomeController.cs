using StorageHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace WebRole.Controllers
{
    public class HomeController : Controller
    {
        QueueHelper queueHelper = new QueueHelper("red");
        BlobHelper blobHelper = new BlobHelper("kontejner");
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Simulacija(string recenica)
        {
            queueHelper.AddToQueue(recenica);
            //Thread.Sleep(3000);


            List<string> recenice = blobHelper.DownloadStringFromBlob("recenice").Split(':').ToList();

            


            return View(recenice);
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