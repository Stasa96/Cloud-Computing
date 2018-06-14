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
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddToQueue(string id)
        {
            queueHelper.AddToQueue(id);

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