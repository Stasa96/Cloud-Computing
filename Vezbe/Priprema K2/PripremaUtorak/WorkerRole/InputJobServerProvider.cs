using Common;
using Microsoft.WindowsAzure.ServiceRuntime;
using StorageHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WorkerRole
{
    public class InputJobServerProvider : IFindFilm
    {
        List<IFindFilm> proxy = new List<IFindFilm>();
        List<Task<Film>> tasks = new List<Task<Film>>();
        public Film FindFilm(string name)
        {
            ConnectToService();
            Film f = null;
            try
            {
                f = WorkerRole.tableHelper.GetOneFilm(name);
                f.Cnt++;
                WorkerRole.tableHelper.AddOrReplaceFilm(f);
            }
            catch
            {
                foreach (IFindFilm p in proxy)
                    tasks.Add(Task<Film>.Factory.StartNew(() => p.FindFilm(name)));

                Task.WaitAll(tasks.ToArray());

                foreach(Task<Film> t in tasks)
                {
                    if (t.Result != null)
                        f = t.Result;
                }
            }
            proxy.Clear();
            return f;
        }

        private void ConnectToService()
        {
            foreach (RoleInstance r in RoleEnvironment.Roles["WorkerRole"].Instances)
            {
                if (r.Id != WorkerRole.id)
                {
                    IPEndPoint add = r.InstanceEndpoints["InternalRequest"].IPEndpoint;
                    ChannelFactory<IFindFilm> factory = new ChannelFactory<IFindFilm>(new NetTcpBinding(), new EndpointAddress($"net.tcp://{add}/InternalRequest"));
                    proxy.Add(factory.CreateChannel());
                }
            }
        }
    }
}
