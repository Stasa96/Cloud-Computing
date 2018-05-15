using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AzureService_data
{
    public class RequestCountInfo : TableEntity
    {
        string instance;
        int requestCnt;

        public RequestCountInfo()
        {
            requestCnt = 0;
        }

        public RequestCountInfo(string instance)
        {
            this.instance = instance;
            RowKey = instance;
            requestCnt = 0;
            PartitionKey = "Request";
        }

        public RequestCountInfo(string instance, int requestCnt)
        {
            PartitionKey = "Request";
            RowKey = instance.ToString();
            this.instance = instance;
            this.requestCnt = requestCnt;
        }

       
        public string Instance { get => instance; set => instance = value; }
        
        public int RequestCnt { get => requestCnt; set => requestCnt = value; }
    }

    [DataContract]
    public class RequestCountInfoWCF
    {
        string instance;
        int requestCnt;

        public RequestCountInfoWCF(string instance, int requestCnt)
        {
            this.instance = instance;
            this.requestCnt = requestCnt;
        }

        RequestCountInfoWCF()
        {

        }

        RequestCountInfoWCF(RequestCountInfo r)
        {
            instance = r.Instance;
            requestCnt = r.RequestCnt;
        }

        [DataMember]
        public string Instance { get => instance; set => instance = value; }
        [DataMember]
        public int RequestCnt { get => requestCnt; set => requestCnt = value; }
    }
}
