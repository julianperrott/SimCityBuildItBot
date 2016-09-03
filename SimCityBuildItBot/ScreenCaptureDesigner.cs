using Common.Logging.Simple;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SimCityBuildItBot.Bot;
using SimplePaletteQuantizer;

namespace SimCityBuildItBot
{
    public partial class ScreenCaptureDesigner : Form
    {
        private CaptureScreen captureScreen;

        public ScreenCaptureDesigner()
        {
            InitializeComponent();
            captureScreen = new CaptureScreen(new NoOpLogger());
        }

        private void btnCapture_Click(object sender, EventArgs e)
        {
            try
            {
                var x = WidthStart.Value;
                var y = heightStart.Maximum - heightStart.Value;
                var s = new Size(WidthEnd.Value - x, heightEnd.Maximum - heightEnd.Value - y);

                this.Text = x + "," + y + ", (" + s.Width + "," + s.Height + ")";

                var image = captureScreen.SnapShot(x, y, s);
                if (this.cbNegative.Checked)
                {
                    image = image.TurnNegative();
                }

                ImageViewer.ShowBitmap(image, this.pictureBox);
                ImageViewer.ShowText(image, this.txtTextFound);

                //ShowBitmap(captureSScreen.SnapShotBuildingTitle("MEmu"), this.pictureBox1, this.pictureBox1Text);
                //ShowBitmap(captureScreen.SnapShotTradeDepotTitle("MEmu"), this.pictureBox2, this.pictureBox2Text);

                //var userDefinedImage = captureScreen.SnapShot("MEmu", int.Parse(txtX.Text), int.Parse(txtY.Text), new Size(int.Parse(txtWidth.Text), int.Parse(txtHeight.Text)));
                //ShowBitmap(userDefinedImage, this.pictureBox3, this.pictureBox3Text);
            }
            catch (Exception ex)
            {
                this.Text = ex.Message;
            }
        }

        private void btnPressAndCapture_Click(object sender, EventArgs e)
        {
            var t = new Touch(new NoOpLogger());
            t.MoveTo(new Point(int.Parse(this.txtX.Text), int.Parse(this.txtY.Text)));

            t.TouchDown();
            t.MoveTo(new Point(int.Parse(this.txtX.Text), int.Parse(this.txtY.Text)));

            Bot.BotApplication.Wait(200);
            btnCapture_Click(sender, e);
            t.TouchUp();
            t.EndTouchData();
        }

        List<Bot.Location> resourceLocations = new List<Bot.Location>()
            {
                Bot.Location.Resources2_Position1,
                Bot.Location.Resources2_Position2,
            };

        private void btnAroundPosition_Click(object sender, EventArgs e)
        {
            var t = new Touch(new NoOpLogger());
            CommerceResourceReader reader = new CommerceResourceReader(new NoOpLogger(), t);


            //pallete1.Entries.ToList().ForEach(s => textBox1.Text += GetClosestColor(colors, s) + ", ");

            var palleteReader = new TwoColourPallette();
            pictureBox1.Image = null;
            pictureBox2.Image = null;
            pictureBox3.Image = null;
            this.textBox1.Text = string.Empty;
            this.textBox2.Text = string.Empty;
            this.textBox3.Text = string.Empty;

            var images = reader.GetResourceImages(Bot.Location.ButtonLeftInner1, resourceLocations);

            pictureBox1.Image = images[0];
            var required = palleteReader.GetClosest2Colours(images[0]).Contains("Red");
            this.textBox1.Text = required ? "Required" : "Ok";

            if (images.Count>1)
            {
                pictureBox2.Image = images[1];
                 required = palleteReader.GetClosest2Colours(images[1]).Contains("Red");
                this.textBox2.Text = required ? "Required" : "Ok";
            }

            if (images.Count > 2)
            {
                pictureBox3.Image = images[2];
                required = palleteReader.GetClosest2Colours(images[2]).Contains("Red");
                this.textBox3.Text = required ? "Required" : "Ok";
            }
        }

        //private void HasResources(Bot.Location clickAt, )
        //{
        //    var t = new Touch(new NoOpLogger());
        //    var item1 = Constants.GetPoint(Bot.Location.ProductionItemLeft1);

        //    t.TouchDown();
        //    t.MoveTo(item1);

        //    ApplicationSleep.Wait(200);

        //    var capturePoint = new Point(int.Parse(this.txtX.Text), int.Parse(this.txtY.Text));
        //    var width = int.Parse(this.txtWidth.Text);
        //    var height = int.Parse(this.txtHeight.Text);

        //    var s = new Size(width, height);

        //    var image = captureScreen.SnapShot("MEmu", capturePoint.X, capturePoint.Y, s);

        //    ImageViewer.ShowBitmap(image, this.pictureBox);

        //    t.TouchUp();
        //    t.EndTouchData();
        //}

        private void txtHeight_TextChanged(object sender, EventArgs e)
        {
        }



        private void Resources1_Click(object sender, EventArgs e)
        {
            resourceLocations = new List<Bot.Location>()
            {
                Bot.Location.Resources1_Position1,
            };
        }

        private void Resources2_Click(object sender, EventArgs e)
        {
            resourceLocations = new List<Bot.Location>()
            {
                Bot.Location.Resources2_Position1,
                Bot.Location.Resources2_Position2,
            };
        }

        private void Resources3_Click(object sender, EventArgs e)
        {
            resourceLocations = new List<Bot.Location>()
            {
                Bot.Location.Resources3_Position1,
                Bot.Location.Resources3_Position2,
                Bot.Location.Resources3_Position3
            };
        }
    }
}