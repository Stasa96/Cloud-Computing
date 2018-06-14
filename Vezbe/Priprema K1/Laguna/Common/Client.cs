using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Client
    {
        static int cnt = 0;
        string fistName;
        string lastName;
        string username;
        string password;
        string id;
        string bankID;



        public Client()
        {

        }

        public Client(string fistName, string lastName, string username, string password, string bankID)
        {
            this.fistName = fistName;
            this.lastName = lastName;
            this.username = username;
            this.password = password;
            this.id = (++cnt).ToString();
            this.bankID = bankID;
        }

        public string FistName { get => fistName; set => fistName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string Id { get => id; set => id = value; }
        public string BankID { get => bankID; set => bankID = value; }
        public string Password { get => password; set => password = value; }
        public string Username { get => username; set => username = value; }
    }
}
