using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureService_data;
using Common;

namespace AzureService_data
{
    public class JobServerProvider : IRequest
    {
        public static string instanceId;
        public static DataRepository repository = new DataRepository();

        public RequestCountInfoWCF Request()
        {
            RequestCountInfo request = repository.GetOneRequest(instanceId);
            request.RequestCnt++;
            repository.AddRequest(request);

            List<RequestCountInfo> requests = repository.GetAllRequest();
            int cnt = 0;
            requests.ForEach(x => { cnt += x.RequestCnt; });
            
            return new RequestCountInfoWCF(instanceId, cnt);
        }
    }
}
