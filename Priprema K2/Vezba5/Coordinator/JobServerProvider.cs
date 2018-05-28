using Common;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Coordinator
{
    public class JobServerProvider : IPurchase
    {
        string roleName;
        string internalEndpointName;
        RoleInstanceEndpoint internalEndpoint;
        IBank bankProxy;
        IBookstore bookstoreProxy;
        List<Task<bool>> tasks = new List<Task<bool>>();

        public JobServerProvider()
        {
            Connect();
        }

        public string ListBooks()
        {
            return bookstoreProxy.ListAvailableItems();
        }

        public string ListUsers()
        {
            return bankProxy.ListClients();
        }

        public bool OrderItem(string bookID, string userID)
        {
            bookstoreProxy.EnlistPurchase(bookID, 1);
            double price = bookstoreProxy.GetItemPrice(bookID);

            if (price == -1)
            {
                return false;
            }

            bankProxy.EnlistMoneyTransfer(userID, price);

            tasks.Add(Task<bool>.Factory.StartNew(() => bookstoreProxy.Prepare()));
            tasks.Add(Task<bool>.Factory.StartNew(() => bankProxy.Prepare()));

            Task.WaitAll(tasks.ToArray());

            if (tasks[0].Result && tasks[1].Result)
            {
                tasks.Clear();
                tasks.Add(Task<bool>.Factory.StartNew(() => { bookstoreProxy.Commit(); return true; }));
                tasks.Add(Task<bool>.Factory.StartNew(() => { bankProxy.Commit(); return true; }));
                Task.WaitAll(tasks.ToArray());
                tasks.Clear();
                return true;
            }
            else
            {
                tasks.Clear();
                tasks.Add(Task<bool>.Factory.StartNew(() => { bookstoreProxy.Rollback(); return true; }));
                tasks.Add(Task<bool>.Factory.StartNew(() => { bankProxy.Rollback(); return true; }));
                Task.WaitAll(tasks.ToArray());
                tasks.Clear();
                return false;
            }
            
        }

        private void Connect()
        {
            //BANK
            roleName = "Bank";
            internalEndpointName = "BankRequest";
            internalEndpoint = RoleEnvironment.Roles[roleName].Instances[0].InstanceEndpoints[internalEndpointName];
            ChannelFactory<IBank> bankFactory = new ChannelFactory<IBank>(new NetTcpBinding(), new EndpointAddress($"net.tcp://{internalEndpoint.IPEndpoint}/{internalEndpointName}"));
            bankProxy = bankFactory.CreateChannel();

            //BOOKSTORE
            roleName = "Bookstore";
            internalEndpointName = "BookstoreRequest";
            internalEndpoint = RoleEnvironment.Roles[roleName].Instances[0].InstanceEndpoints[internalEndpointName];
            ChannelFactory<IBookstore> bookstoreFactory = new ChannelFactory<IBookstore>(new NetTcpBinding(), new EndpointAddress($"net.tcp://{internalEndpoint.IPEndpoint}/{internalEndpointName}"));
            bookstoreProxy = bookstoreFactory.CreateChannel();
        }
    }
}
