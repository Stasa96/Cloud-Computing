using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Book
    {
        string id;
        string name;
        int count;
        double price;
        static int cnt = 0;

        public Book(string name, int count, double price)
        {
            this.Id = (++cnt).ToString();
            this.Name = name;
            this.Count = count;
            this.Price = price;
        }
        public Book()
        {

        }

        public string Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public int Count { get => count; set => count = value; }
        public double Price { get => price; set => price = value; }
    }
}
