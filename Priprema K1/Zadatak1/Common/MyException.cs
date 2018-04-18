using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class MyException
    {
        string reason;

        public MyException(string reason)
        {
            this.Reason = reason;
        }

        public MyException()
        {

        }

        [DataMember]
        public string Reason { get => reason; set => reason = value; }
    }
}
