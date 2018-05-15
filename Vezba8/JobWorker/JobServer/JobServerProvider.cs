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

            RequestCountInfoWCF r = new RequestCountInfoWCF(instanceId, requests[0].RequestCnt + requests[1].RequestCnt);

            return r;
        }
    }
}
