namespace SimCityBuildItBot
{
    using Common.Logging.Simple;
    using SimCityBuildItBot.Bot;
    using System;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    using Tesseract;

    public partial class DebugForm : Form
    {
        private CaptureScreen captureScreen;

        public DebugForm()
        {
            InitializeComponent();

            this.BackgroundImageLayout = ImageLayout.Center;

            txtX.Text = "420";
            txtY.Text = "100";
            txtWidth.Text = "836";
            txtHeight.Text = "88";

            captureScreen = new CaptureScreen(new NoOpLogger());
        }

        private Image ToImage(Bitmap bitmap)
        {
            MemoryStream memoryStream = new MemoryStream();
            bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
            return Image.FromStream(memoryStream);
        }

        private void ShowBitmap(Bitmap bitmap, PictureBox pictureBox, TextBox textBox)
        {
            textBox.Text = "";
            pictureBox.BackgroundImage = null;

            if (bitmap != null)
            {
                pictureBox.BackgroundImage = ToImage(bitmap);

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

        public Bitmap SnapShotBuildingTitle(string procName)
        {
            return captureScreen.SnapShot(procName, 660, 120, new Size(550, 70));
        }

        public Bitmap SnapShotTradeDepotTitle(string procName)
        {
            return captureScreen.SnapShot(procName, 410, 90, new Size(400, 70));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                ShowBitmap(SnapShotBuildingTitle("MEmu"), this.pictureBox1, this.pictureBox1Text);
                ShowBitmap(SnapShotTradeDepotTitle("MEmu"), this.pictureBox2, this.pictureBox2Text);

                var userDefinedImage = captureScreen.SnapShot("MEmu", int.Parse(txtX.Text), int.Parse(txtY.Text), new Size(int.Parse(txtWidth.Text), int.Parse(txtHeight.Text)));
                ShowBitmap(userDefinedImage, this.pictureBox3, this.pictureBox3Text);
            }
            catch (Exception ex)
            {
                this.Text = ex.Message;
            }
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            //  Mouse.LeftMouseClick(543, 170);
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            // Mouse.LeftMouseClick(1344, 160);
        }

        private void btnCaptureDesigner_Click(object sender, EventArgs e)
        {
            new ScreenCaptureDesigner().ShowDialog();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            new TestForm().ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new ResourceReaderDesigner().ShowDialog();
        }

        private void btnTradeDepot_Click(object sender, EventArgs e)
        {
            new TradeDepot().ShowDialog();
        }
    }
}