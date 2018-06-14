using Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class BankServerProvider : IBankInteraction
    {
        static Dictionary<string, double> accounts = new Dictionary<string, double>();
        public bool AddBankAccount(string bankId, double amount)
        {
            if (!accounts.ContainsKey(bankId))
            {
                accounts.Add(bankId, amount);
                Trace.WriteLine($"Account added {bankId} {amount}");
                return true;
            }
            return false;
        }

        public bool Deposit(string bankId, double amount)
        {
            if (accounts.ContainsKey(bankId))
            {
                accounts[bankId] += amount;
                Trace.WriteLine($"Deposit {bankId} {amount}");
                return true;
            }
            return false;
        }

        public string info(string bankId)
        {
            if (accounts.ContainsKey(bankId))
            {
                Trace.WriteLine($"BankID: {bankId}, Amount: {accounts[bankId]}");
                return $"BankID: {bankId}, Amount: {accounts[bankId]}";
            }
            return "Error";
        }

        public bool Withdraw(string bankId, double amount)
        {
            if (accounts.ContainsKey(bankId) && accounts[bankId] >= amount)
            {
                accounts[bankId] -= amount;
                Trace.WriteLine($"Withdraw {bankId} {amount}");
                
                return true;
            }
            return false;
        }
    }
}
