using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageHelper
{
    // Promeniti u zavisnosti od modela
    public class Film : TableEntity
    {
        string naziv;
        string photoURL;
        public Film() { }
        public Film(string naziv, string photoUrl)
        {
            PartitionKey = "Film";
            RowKey = naziv;
            this.naziv = naziv;
            this.photoURL = photoUrl;
        }

        public string Naziv { get => naziv; set => naziv = value; }
        public string PhotoURL { get => photoURL; set => photoURL = value; }
    }
}
