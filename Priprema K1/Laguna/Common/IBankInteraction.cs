using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IBankInteraction
    {
        [OperationContract]
        bool AddBankAccount(string bankId, double amount);
        [OperationContract]
        bool Deposit(string bankId, double amount);
        [OperationContract]
        bool Withdraw(string bankId,double amount);
        [OperationContract]
        string info(string bankId);
    }
}
