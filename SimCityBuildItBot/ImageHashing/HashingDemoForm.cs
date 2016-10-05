using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimCityBuildItBot.ImageHashing
{
    public partial class HashingDemoForm : Form
    {
        public HashingDemoForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var image = this.pictureBox1.Image;

            Bitmap squeezed = new Bitmap(8, 8, PixelFormat.Format32bppRgb);
            Graphics canvas = Graphics.FromImage(squeezed);
            canvas.CompositingQuality = CompositingQuality.HighQuality;
            canvas.InterpolationMode = InterpolationMode.HighQualityBilinear;
            canvas.SmoothingMode = SmoothingMode.HighQuality;
            canvas.DrawImage(image, 0, 0, 8, 8);

        

            var image2 = new Bitmap(128, 128);
            var image3 = new Bitmap(128, 128);
            var image4 = new Bitmap(128, 128);


            // Reduce colors to 6-bit grayscale and calculate average color value
            byte[] grayscale = new byte[64];
            uint averageValue = 0;
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    uint pixel = (uint)squeezed.GetPixel(x, y).ToArgb();


                    for(int xx=0;xx<256;xx++)
                    {
                        image2.SetPixel((x*16)+(xx % 16) , (y*16)+((int)(xx/16)), Color.FromArgb(squeezed.GetPixel(x, y).ToArgb()));
                    }

                    
                    uint gray = (pixel & 0x00ff0000) >> 16;
                    gray += (pixel & 0x0000ff00) >> 8;
                    gray += (pixel & 0x000000ff);
                    gray /= 12;

                    grayscale[x + (y * 8)] = (byte)gray;


                    var g = ((int)gray) * 4;
                    for (int xx = 0; xx < 256; xx++)
                    {
                        image3.SetPixel((x * 16) + (xx % 16), (y * 16) + ((int)(xx / 16)), Color.FromArgb(g,g,g));
                    }

                    averageValue += gray;
                }
            }
            averageValue /= 64;

            if (string.IsNullOrEmpty(this.txtAverage.Text))
            {
                this.txtAverage.Text = averageValue.ToString();
            }
            else
            {
                averageValue = uint.Parse(this.txtAverage.Text);
            }

            this.pictureBox2.Image = image2;
            this.pictureBox3.Image = image3;


            canvas = Graphics.FromImage(image4);
            canvas.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
            string value = "";

            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    var color = Color.Black;
                    var brush = Brushes.White;
                    var text = "0";
                    if (grayscale[x + (y * 8)] >= averageValue)
                    {
                        color = Color.White;
                        brush = Brushes.Black;
                        text = "1";
                    }

                    value += text;



                    for (int xx = 0; xx < 256; xx++)
                    {
                        image4.SetPixel((x * 16) + (xx % 16), (y * 16) + ((int)(xx / 16)), color);
                    }

                    RectangleF rectf = new RectangleF(x * 16, y * 16, (x * 16)+15, (y * 16)+15);
                    canvas.DrawString(text, new Font("Tahoma", 8), brush, rectf);
                }
            }

            this.pictureBox4.Image = image4;

            // Compute the hash: each bit is a pixel
            // 1 = higher than average, 0 = lower than average
            ulong hash = 0;
            for (int i = 0; i < 64; i++)
            {
                if (grayscale[i] >= averageValue)
                {
                    hash |= (1UL << (63 - i));
                }
            }

            this.Text =  value;
            this.textBox1.Text = value;
            this.textBox2.Text = hash.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";

            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            this.pictureBox1.Image = Image.FromStream(myStream);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.textBox4.Text = ((int)ImageHashing.Similarity(ulong.Parse(this.textBox2.Text), ulong.Parse(this.textBox3.Text))).ToString()+"%";
        }
    }
}
