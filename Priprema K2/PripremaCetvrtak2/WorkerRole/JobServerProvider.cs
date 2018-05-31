using Common;
using StorageHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerRole
{
    public class JobServerProvider : INotify
    {
        static string text = "";
        static string redText = "";
        BlobHelper blobHelper = new BlobHelper("kontejner");
        QueueHelper queueHelper = new QueueHelper("red2");
        public void Notify(string s)
        {
            s += "\n";
            redText = s;

            if (s.Split('_')[2] == "OK")
            {
                text += s;
                blobHelper.UploadStringToBlob("porudzbine", text);
            }

            queueHelper.AddToQueue(redText);
        }
    }
}
