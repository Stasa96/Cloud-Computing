using StorageHelper;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public ActionResult Istorija(string naziv)
        {
            string s = "Ne postoji dati naziv";
            try
            {
               s = blobHelper.DownloadStringFromBlob(naziv);
            }
            catch { }
            ViewBag.Stanja = s;
            return View();
        }

        [HttpPost]
        public ActionResult Izvrsi(string naziv,string stanje)
        {
            queueHelper.AddToQueue($"{naziv}:{stanje}");

            try
            {

                string s = blobHelper.DownloadStringFromBlob(naziv);
                s += stanje + "_";
                blobHelper.UploadStringToBlob(naziv, s);

            }
            catch
            {
                blobHelper.UploadStringToBlob(naziv, stanje+"_");
            }

            return View("Index");
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