using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Client
    {
        string username;
        string id;
        double amount;

        public Client(string username, string id, double amount)
        {
            this.username = username;
            this.id = id;
            this.amount = amount;
        }

        public Client()
        {

        }

        public string Username { get => username; set => username = value; }
        public string Id { get => id; set => id = value; }
        public double Amount { get => amount; set => amount = value; }
    }
}
