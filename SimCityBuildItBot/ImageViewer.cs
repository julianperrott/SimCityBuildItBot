using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tesseract;

namespace SimCityBuildItBot
{
    public static class ImageViewer
    {
        public static Image ToImage(Bitmap bitmap)
        {
            MemoryStream memoryStream = new MemoryStream();
            bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
            return Image.FromStream(memoryStream);
        }

        public static void ShowBitmap(Bitmap bitmap, PictureBox pictureBox)
        {
            pictureBox.BackgroundImage = null;

            if (bitmap != null)
            {
                pictureBox.BackgroundImage = ToImage(bitmap);
            }
        }

        public static void ShowText(Bitmap bitmap, TextBox textBox)
        {
            textBox.Text = "";
            using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
            {
                using (var pix = new BitmapToPixConverter().Convert(bitmap))
                {
                    using (var page = engine.Process(pix, PageSegMode.SingleLine))
                    {
                        textBox.Text = page.GetText().Trim();
                    }
                }
            }
        }
    }
}

