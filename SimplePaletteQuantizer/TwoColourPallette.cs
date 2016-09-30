using SimplePaletteQuantizer.ColorCaches;
using SimplePaletteQuantizer.ColorCaches.Common;
using SimplePaletteQuantizer.ColorCaches.EuclideanDistance;
using SimplePaletteQuantizer.Helpers;
using SimplePaletteQuantizer.Quantizers;
using SimplePaletteQuantizer.Quantizers.DistinctSelection;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace SimplePaletteQuantizer
{
    public class TwoColourPallette
    {
        public static Dictionary<string, Color> Colors = new Dictionary<string, Color>()
            {
                {"Red",Color.Red },
                {"Green",Color.Green},
                {"Blue",Color.Blue},
                {"White",Color.White},
                {"Black",Color.Black},
                {"DarkGray",Color.DarkGray},
                {"LightGray",Color.LightGray},
                {"Yellow",Color.Yellow},
                {"Violet",Color.Violet},
            };

        IColorQuantizer activeQuantizer = new DistinctSelectionQuantizer();
        IColorCache activeColorCache = new EuclideanDistanceColorCache();

        public TwoColourPallette()
        {
            ((BaseColorCacheQuantizer)activeQuantizer).ChangeCacheProvider(activeColorCache);
            ((BaseColorCache)activeColorCache).ChangeColorModel(ColorModel.RedGreenBlue);
        }

        private static Image ToImage(Bitmap bitmap)
        {
            MemoryStream memoryStream = new MemoryStream();
            bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
            return Image.FromStream(memoryStream);
        }

        public List<string> GetClosest2Colours(Bitmap sourceImage)
        {
            return GetClosest2Colours(sourceImage, Colors);
        }

        public List<string> GetClosest2Colours(Bitmap sourceImage, Dictionary<string, Color> colors)
        {
            Int32 parallelTaskCount = 1;
            Image targetImage = ImageBuffer.QuantizeImage(ToImage(sourceImage), activeQuantizer, null, 2, parallelTaskCount);
            return targetImage.Palette.Entries.Select(e => GetClosestColor(colors, e)).ToList();
        }

        private static string GetClosestColor(Dictionary<string, Color> colors, Color baseColor)
        {
            var colorDiffs = colors
                        .Select(x => new { Value = x.Key, Diff = GetDiff(x.Value, baseColor) })
                        .ToList();

            var min = colorDiffs.Min(x => x.Diff);

            return colorDiffs.Find(x => x.Diff == min).Value;
        }

        private static int GetDiff(Color color, Color baseColor)
        {
            int a = color.A - baseColor.A,
                r = color.R - baseColor.R,
                g = color.G - baseColor.G,
                b = color.B - baseColor.B;
            return a * a + r * r + g * g + b * b;
        }

    }
}
