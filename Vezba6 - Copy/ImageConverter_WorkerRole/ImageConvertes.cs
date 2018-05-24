using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ImageConverter_WorkerRole
{
    class ImageConvertes
    {
        public static System.Drawing.Image ConvertImage(System.Drawing.Image img)
        {
            return (System.Drawing.Image)(new Bitmap(img, new Size(20, 20)));
        }
    }
}
