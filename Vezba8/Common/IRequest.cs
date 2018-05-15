using System;
using AzureService_data;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IRequest
    {
        [OperationContract]
        RequestCountInfoWCF Request();
    }
}
