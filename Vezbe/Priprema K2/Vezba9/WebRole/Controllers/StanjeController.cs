using Common;
using StorageHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace WebRole.Controllers
{
    public class StanjeController : Controller
    {
        IBrotherConnection proxy;
        QueueHelper queueHelper = new QueueHelper("red");
        BlobHelper blobHelper = new BlobHelper("kontejner");
        // GET: Stanje
        public ActionResult Index()
        {
            ViewBag.Stanje = "ZATVORENO";
            return View();
        }

        public ActionResult AddToQueue(string stanje)
        {
            queueHelper.AddToQueue(stanje);
            Thread.Sleep(1000);
            ViewBag.Stanje = blobHelper.DownloadStringFromBlob("stanje");
            return View("Index");
        }

        public ActionResult Check()
        {
            Connect();

            bool isAlive = proxy.AreYouAlive();

            if (isAlive)
                ViewBag.Alive = "BROTHER IS ALIVE.";
            else
                ViewBag.Alive = "BROTHER IS NOT ALIVE";

            return View("Index");
        }

        private void Connect()
        {
            ChannelFactory<IBrotherConnection> factory = new ChannelFactory<IBrotherConnection>(new NetTcpBinding(), new EndpointAddress($"net.tcp://127.255.0.2:8888/InputRequest"));
            proxy = factory.CreateChannel();
        }
    }
}