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
        double price;
        uint count;

        
        public Book()
        {

        }

        public Book(string id, string name, double price, uint count)
        {
            this.id = id;
            this.name = name;
            this.price = price;
            this.count = count;
        }

        public string Name { get => name; set => name = value; }
        public double Price { get => price; set => price = value; }
        public uint Count { get => count; set => count = value; }
        public string Id { get => id; set => id = value; }
    }
}
