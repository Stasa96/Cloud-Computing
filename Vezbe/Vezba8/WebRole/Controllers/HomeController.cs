using AzureService_data;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;

namespace WebRole.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ChannelFactory<IRequest> factory = new ChannelFactory<IRequest>(new NetTcpBinding(),new EndpointAddress("net.tcp://localhost:11001/InputRequest"));
            IRequest proxy = factory.CreateChannel();

            RequestCountInfoWCF r = proxy.Request();

            return View(r);
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