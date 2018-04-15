using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IClientInteraction
    {
        [OperationContract]
        bool Registration(string username, string firstName, string lastName, string password, string bankId);

        [OperationContract]
        bool OrderBook(string bookName,string username,string password,int count);

        [OperationContract]
        bool ReturBook(string bookName, string username, string password, int count);

        [OperationContract]
        bool AddBook(string bookName,int count,double price);
    }
}
