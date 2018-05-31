using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageHelper
{
    public class Proizvod :TableEntity
    {
        string naziv;
        int kolicina;

        public Proizvod() { }
        public Proizvod(string naziv, int kolini)
        {
            PartitionKey = "Proizvod";
            RowKey = naziv;
            this.naziv = naziv;
            this.kolicina = kolini;
        }

        public string Naziv { get => naziv; set => naziv = value; }
        public int Kolicina { get => kolicina; set => kolicina = value; }
    }
}
