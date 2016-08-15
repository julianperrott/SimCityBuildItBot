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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimplePaletteQuantizer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainFormLoad(object sender, EventArgs e)
        {
            Image sourceImage = Image.FromFile(@"C:\\Users\\JP\\Desktop\\ok.png");
            var pallete1 = GetColours(sourceImage, pictureTarget1);

            Image sourceImage2 = Image.FromFile(@"C:\\Users\\JP\\Desktop\\bad.png");
            var pallete2 = GetColours(sourceImage2, pictureTarget2);

            Dictionary<string, Color> colors = new Dictionary<string, Color>()
            {
                { "Red",Color.Red },
                {"Green",Color.Green},
                {"Blue",Color.Blue},
                {"White",Color.White},
                {"Black",Color.Black},
                {"DarkGray",Color.DarkGray},
                {"LightGray",Color.LightGray},
                {"Yellow",Color.Yellow},
                {"Violet",Color.Violet},
            };

            colors.Clear();
            Enumerable.Range(28, 167 - 28).ToList().ForEach(s =>
                {
                    colors.Add(((KnownColor)s).ToString(), Color.FromKnownColor((KnownColor)s));
                });

            textBox1.Text = "";
            textBox2.Text = "";
            pallete1.Entries.ToList().ForEach(s => textBox1.Text += GetClosestColor(colors, s) + ", ");
            pallete2.Entries.ToList().ForEach(s => textBox2.Text += GetClosestColor(colors, s) + ", ");
        }

        private ColorPalette GetColours(Image sourceImage, PictureBox picture)
        {
            IColorQuantizer activeQuantizer = new DistinctSelectionQuantizer();
            IColorCache activeColorCache = new EuclideanDistanceColorCache();

            ((BaseColorCacheQuantizer)activeQuantizer).ChangeCacheProvider(activeColorCache);
            ((BaseColorCache)activeColorCache).ChangeColorModel(ColorModel.RedGreenBlue);

            Int32 parallelTaskCount = 8;
            TaskScheduler uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();

            Image targetImage = ImageBuffer.QuantizeImage(sourceImage, activeQuantizer, null, 2, parallelTaskCount);
            picture.Image = targetImage;
            return targetImage.Palette;
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