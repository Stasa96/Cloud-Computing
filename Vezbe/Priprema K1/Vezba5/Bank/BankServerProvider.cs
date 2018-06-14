using Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    public class BankServerProvider : IBank
    {
        static Dictionary<string, Client> clients = new Dictionary<string, Client>() { { "123", new Client("stefan", "123", 61000) }, { "1234", new Client("stefan1", "1234", 10900) }, { "1235", new Client("stefani", "1235", 4000) } };
        public void EnlistMoneyTranfer(string userID, double amount)
        {
            Client c;

            if(clients.TryGetValue(userID,out c))
            {
                if(c.Amount >= amount)
                {
                    clients[userID].Amount -= amount;
                    Trace.WriteLine($"{c.Id} {c.Username} spent {amount}");
                    ListClients();
                }
                else
                {
                    Trace.WriteLine($"{c.Id} {c.Username} doesn't have enough money");
                }
            }
            else
            {
                Trace.WriteLine($"Client doesn't exists.");
            }
        }

        public void ListClients()
        {
            foreach(Client c in clients.Values)
            {
                Trace.WriteLine("-----------------------");
                Trace.WriteLine($"ID: {c.Id}\nName: {c.Username}\nPrice: {c.Amount}");
                Trace.WriteLine("-----------------------");
            }
        }
    }
}
