using ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Worker
{
    public class WorkerRole : IWorkerRole
    {
        int ID;
        public void Start(string containerId)
        {
            ID = int.Parse(containerId);
            calculate();
            Stop();
        }

        public void Stop()
        {
            Console.WriteLine("_______________________________________________\nWorkerRole Dll is Stoped.");
        }

        private void Begin()
        {
            int i = 20;
            while (i-- > 0)
            {
                //Console.WriteLine($"Working on container with port {ID}...");
                calculate();
                //Thread.Sleep(1000);
            }
        }

        private void calculate()
        {
            Console.WriteLine("CALCULATOR");
            Console.WriteLine("------------------------------------------");
            Console.WriteLine("Enter first number");
            int first = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter second number");
            int second = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter operation(ADD,SUB,DIV,MUL)");
            string op = Console.ReadLine();

            switch (op.ToUpper())
            {
                case "ADD":
                    Console.WriteLine($"{first} + {second} = {first + second}");
                    break;
                case "SUB":
                    Console.WriteLine($"{first} - {second} = {first - second}");
                    break;
                case "MUL":
                    Console.WriteLine($"{first} * {second} = {first * second}");
                    break;
                case "DIV":
                    Console.WriteLine($"{first} / {second} = {(double)((double)first / (double)second)}");
                    break;
                default:
                    break;
            }
        }
    }
}
