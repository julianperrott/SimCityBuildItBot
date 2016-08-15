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
    public partial class ResourceReaderDesigner : Form
    {
        private CaptureScreen captureScreen;

        public ResourceReaderDesigner()
        {
            InitializeComponent();
            captureScreen = new CaptureScreen(new NoOpLogger());
        }



        List<Bot.Location> resourceLocations = new List<Bot.Location>()
            {
                Bot.Location.Resources2_Position1,
                Bot.Location.Resources2_Position2,
            };

        private void ClickAndGetResources(Bot.Location location)
        {
            var t = new Touch(new NoOpLogger());
            CommerceResourceReader reader = new CommerceResourceReader(new NoOpLogger(), t);

            var palleteReader = new TwoColourPallette();
            pictureBox1.Image = null;
            pictureBox2.Image = null;
            pictureBox3.Image = null;
            this.textBox1.Text = string.Empty;
            this.textBox2.Text = string.Empty;
            this.textBox3.Text = string.Empty;

            var images = reader.GetResourceImages(location, resourceLocations);

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

        private void btnLeft1_Click(object sender, EventArgs e)
        {
            ClickAndGetResources(Bot.Location.ButtonLeftInner1);
        }

        private void btnLeft2_Click(object sender, EventArgs e)
        {
            ClickAndGetResources(Bot.Location.ButtonLeftInner2);
        }

        private void btnLeft3_Click(object sender, EventArgs e)
        {
            ClickAndGetResources(Bot.Location.ButtonLeftInner3);
        }

        private void btnRight1_Click(object sender, EventArgs e)
        {
            ClickAndGetResources(Bot.Location.ButtonRightInner1);
        }

        private void btnRight2_Click(object sender, EventArgs e)
        {
            ClickAndGetResources(Bot.Location.ButtonRightInner2);
        }

        private void btnRight3_Click(object sender, EventArgs e)
        {
            ClickAndGetResources(Bot.Location.ButtonRightInner3);
        }
    }
}