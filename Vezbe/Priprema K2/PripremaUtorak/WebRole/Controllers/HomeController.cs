using Common;
using Microsoft.WindowsAzure.ServiceRuntime;
using StorageHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;

namespace WebRole.Controllers
{
    public class HomeController : Controller
    {
        BlobHelper blobHelper = new BlobHelper("filmovi");
        QueueHelper queueHelper = new QueueHelper("red");
        IFindFilm proxy;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Find()
        {
            return View();
        }

        public ActionResult FindFilm(string naziv)
        {
            Connect();

            Film f = proxy.FindFilm(naziv);

            if(f == null)
            {
                return View("Greska");
            }

            return View("About",f);
        }

        [HttpPost]
        public ActionResult AddEntity(string Naziv, HttpPostedFileBase file)
        {
            try
            {
                blobHelper.UploadFileToBlob(Naziv, file);
                queueHelper.AddToQueue(Naziv);
                return View("Index");
                
            }
            catch
            {
                return View("Greska");
            }
          
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

        private void Connect()
        {
            ChannelFactory<IFindFilm> factory = new ChannelFactory<IFindFilm>(new NetTcpBinding(), new EndpointAddress($"net.tcp://localhost:8888/InputRequest"));
            proxy = factory.CreateChannel();
        }
    }

   
}