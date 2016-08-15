using System.Drawing;
using System.Drawing.Imaging;

namespace SimCityBuildItBot
{
    public static class BitmapExtension
    {
        public static Bitmap TurnNegative(this Bitmap source)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(source.Width, source.Height);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);

            // create the negative color matrix

            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][]
               {
                      new float[] {-1, 0, 0, 0, 0},
                      new float[] {0, -1, 0, 0, 0},
                      new float[] {0, 0, -1, 0, 0},
                      new float[] {0, 0, 0, 1, 0},
                      new float[] {1, 1, 1, 0, 1}
               });

            // create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            attributes.SetColorMatrix(colorMatrix);

            g.DrawImage(source, new Rectangle(0, 0, source.Width, source.Height),
                        0, 0, source.Width, source.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();

            return newBitmap;
        }
    }
}