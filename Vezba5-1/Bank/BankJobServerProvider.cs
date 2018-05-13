using Bank_Data;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    public class BankJobServerProvider : IBank
    {
        BankDataRepository repository = new BankDataRepository();
        public void Commit()
        {
            throw new NotImplementedException();
        }

        public void EnlistMoneyTransfer(string userID, double amount)
        {
            throw new NotImplementedException();
        }

        public string ListClients()
        {
            throw new NotImplementedException();
        }

        public bool Prepare()
        {
            throw new NotImplementedException();
        }

        public void Rollback()
        {
            throw new NotImplementedException();
        }
    }
}
