using Common;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Coordinator
{
    public class CoordinatorServerProvider : IClientInteraction
    {
        static Dictionary<string, Client> clients = new Dictionary<string, Client>();
        IBankInteraction bank;
        IBookstoreInteraction bookstore;

        public bool AddBook(string bookName, int count, double price)
        {
            Trace.WriteLine($"Adding {bookName}");
            Connect();
            if (bookstore.AddBook(bookName, count, price) == -2)
            {
                Trace.WriteLine($"{bookName} added");
                return true;
            }

            Dissconect();

           return false;
        }

        public bool OrderBook(string bookName, string username, string password, int count)
        {
            Trace.WriteLine($"Ordering {bookName} for {username}");
            Connect();
            string flag = null;
            foreach(Client c in clients.Values)
            {
                if(c.Username == username && c.Password == password)
                {
                    flag = c.Id;
                    Trace.WriteLine($"Ordering {bookName} for {username}");

                    double price = bookstore.GetPrice(bookName);
                    if(bank.Withdraw(c.BankID, price * count))
                    {
                        bookstore.removeBook(bookName,count);
                        Trace.WriteLine($"Ordered {bookName} for {username}");
                        return true;
                    }
                }
            }

            if(flag == null)
            {
                return false;
            }
            Dissconect();
            return false;
        }

        public bool Registration(string username, string firstName, string lastName, string password, string bankId)
        {
            Trace.WriteLine($"Registration started for {username}");
            Connect();
            foreach (Client c in clients.Values)
            {
                if (c.Username == username)
                {
                    return false;
                }
            }
            Client cl = new Client(firstName,lastName,username,password,bankId);
            clients.Add(cl.Id, cl);
            bank.AddBankAccount(cl.BankID, 50000);
            Dissconect();
            Trace.WriteLine($"Registration completed for {username}");
            return true;
        }

        public bool ReturBook(string bookName, string username, string password, int count)
        {
            Trace.WriteLine($"Returning {bookName} for {username}");
            Connect();
            string flag = null;
            foreach (Client c in clients.Values)
            {
                if (c.Username == username && c.Password == password)
                {
                    flag = c.Id;

                    double price = bookstore.GetPrice(bookName);
                    if (bank.Deposit(c.BankID, price * count))
                    {
                        bookstore.AddBook(bookName, count,0);
                        Trace.WriteLine($"{bookName} returned for {username}");
                        return true;
                    }
                }
            }
            Dissconect();
            if (flag == null)
            {
                return false;
            }
            return false;
        }

        private void Connect()
        {
            Trace.WriteLine($"Connectiong");
            try
            {
                RoleInstanceEndpoint bankEndpoint = RoleEnvironment.Roles["Bank"].Instances[0].InstanceEndpoints["InternalBank"];
                ChannelFactory<IBankInteraction> bankFactory = new ChannelFactory<IBankInteraction>(new NetTcpBinding(), new EndpointAddress($"net.tcp://{bankEndpoint.IPEndpoint}/InternalBank"));
                bank = bankFactory.CreateChannel();

                RoleInstanceEndpoint bookstoreEndpoint = RoleEnvironment.Roles["Bookstore"].Instances[0].InstanceEndpoints["InternalBookstore"];
                ChannelFactory<IBookstoreInteraction> bookstoreFactory = new ChannelFactory<IBookstoreInteraction>(new NetTcpBinding(), new EndpointAddress($"net.tcp://{bookstoreEndpoint.IPEndpoint}/InternalBookstore"));
                bookstore = bookstoreFactory.CreateChannel();
                Trace.WriteLine($"Connected bank and bookstore");
            }
            catch (Exception e)
            {
                bank = null;
                bookstore = null;
                Trace.WriteLine($"Error");
            }

        }

        private void Dissconect()
        {
            bank = null;
            bookstore = null;
            Trace.WriteLine($"Disconected");
        }
    }
}
