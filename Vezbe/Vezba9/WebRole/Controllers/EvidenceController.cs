using CloudService_Data;
using Common;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;

namespace WebRole.Controllers
{
    public class EvidenceController : Controller
    {
        // GET: Evidence
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Simulation(string input)
        {
            if(!input.ToUpper().Equals("OTVORENO") && !input.ToUpper().Equals("ZATVORENO"))
            {
                ViewBag.ErrorMessage = "Vrednost koju ste uneli nije validna,validne poruke-> OTVORENO/ZATVORENO";
                return View("Error");
            }

            CloudQueue queue = QueueHelper.GetQueueReference("queue");
            queue.AddMessage(new CloudQueueMessage(input.ToUpper()));

            return View("Index");
        }

        public ActionResult ConnectTwoBrothers()
        {
            string response = null;

            try
            {
                IControlServiceHost proxy = Connect();
                response = proxy.IsYourBrotherThere();
            }
            catch(Exception e)
            {
                response = "Instance 0 is not alive";
            }

            ViewBag.Response = response;

            return View("Index");
        }

        public ActionResult Table()
        {
            TableHelper table = new TableHelper();
            List<EvidenceEntity> content = table.RetrieveAllEvidence();

            return View("Table", content.OrderBy(entiti => int.Parse(entiti.RowKey)));
        }

        private IControlServiceHost Connect()
        {
            NetTcpBinding binding = new NetTcpBinding()
            {
                SendTimeout = new TimeSpan(0, 10, 0),
                OpenTimeout = new TimeSpan(0, 10, 0),
                CloseTimeout = new TimeSpan(0, 10, 0),
                ReceiveTimeout = new TimeSpan(0, 10, 0),

            };
            //RoleInstanceEndpoint remoteInstanceEP = RoleEnvironment.Roles["WorkerRole"].Instances.Where(i => i.Id.Split('.').Last().Split('_').Last().Equals("1")).First().InstanceEndpoints["InputRequest"];

           // String remoteAddress = $"net.tcp://{remoteInstanceEP.IPEndpoint}/InputRequest";
           String remoteAddress = $"net.tcp://127.255.0.2:11001/InputRequest";

            return new ChannelFactory<IControlServiceHost>(binding, remoteAddress).CreateChannel();

        }
    }
}
