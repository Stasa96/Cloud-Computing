using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IBookstore : ITransacion
    {
        [OperationContract]
        string ListAvailableItems();
        [OperationContract]
        void EnlistPurchase(string bookID, uint count);
        [OperationContract]
        double GetItemPrice(string bookID);
    }
    [ServiceContract]
    public interface IBank : ITransacion
    {
        [OperationContract]
        string ListClients();
        [OperationContract]
        void EnlistMoneyTransfer(string userID, double amount);
    }
    [ServiceContract]
    public interface IPurchase 
    {
        [OperationContract]
        bool OrderItem(string bookID, string userID);
    }
    [ServiceContract]
    public interface ITransacion
    {
        [OperationContract]
        bool Prepare();
        [OperationContract]
        void Commit();
        [OperationContract]
        void Rollback();
    }


}
