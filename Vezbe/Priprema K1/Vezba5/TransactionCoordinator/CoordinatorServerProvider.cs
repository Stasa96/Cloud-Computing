using Common;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace TransactionCoordinator
{
    class CoordinatorServerProvider : IPurchase
    {
        IBank bank;
        IBookstore bookstore;

        public bool orderItem(string bookID, string userID)
        {
            Connect();
            Trace.WriteLine($"Connecting to Bank and Bookstore.");
            bookstore.ListAvailableItems();
            double amount = bookstore.GetItemPrice(bookID);
            Trace.WriteLine($"Book costs {amount}");
            bank.ListClients();
            bank.EnlistMoneyTranfer(userID,amount);
            Trace.WriteLine($"Money transefer completed.");
            bookstore.EnlistPurchase(bookID, 1);
            Trace.WriteLine($"Book is purchased.");


            return true;
        }

        private void Connect()
        {
            RoleInstanceEndpoint bookstoreEndpoint = RoleEnvironment.Roles["Bookstore"].Instances[0].InstanceEndpoints["InternalBookstore"];
            ChannelFactory<IBookstore> bookstoreFactory = new ChannelFactory<IBookstore>(new NetTcpBinding(), new EndpointAddress($"net.tcp://{bookstoreEndpoint.IPEndpoint}/InternalBookstore"));
            bookstore = bookstoreFactory.CreateChannel();

            RoleInstanceEndpoint bankEndpoint = RoleEnvironment.Roles["Bank"].Instances[0].InstanceEndpoints["InternalBank"];
            ChannelFactory<IBank> bankFactory = new ChannelFactory<IBank>(new NetTcpBinding(), new EndpointAddress($"net.tcp://{bankEndpoint.IPEndpoint}/InternalBank"));
            bank = bankFactory.CreateChannel();
            
            
        }
    }
}
