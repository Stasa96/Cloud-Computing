using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IBookstoreInteraction
    {
        [OperationContract]
        double AddBook(string bookName,int count,double price);
        [OperationContract]
        double removeBook(string bookName, int count);
        [OperationContract]
        double GetPrice(string bookName);
    }
}
