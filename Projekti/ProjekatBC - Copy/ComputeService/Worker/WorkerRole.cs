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
            Begin();
            Stop();
        }

        public void Stop()
        {
            Console.WriteLine("_______________________________________________\nWorkerRole Dll is Stoped.");
        }

        private void Begin()
        {
            while (true)
            {
                Console.WriteLine($"Working on container with port {ID}...");
            
                Thread.Sleep(1000);
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
