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
        QueueHelper queueHelper2 = new QueueHelper("red2");
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddToQueue(string vrsta,string kolicina)
        {
            try
            {
                int.Parse(kolicina);
                string msg = $"{vrsta}_{kolicina}";
                queueHelper.AddToQueue(msg);

                return View("Index");
            }
            catch
            {
                return View("Greska");
            }
        }

        public ActionResult GetFromBlob()
        {
            string s;
            while ((s = queueHelper2.GetFromQueue()) != null)
                ViewBag.Porudzbine += s;

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