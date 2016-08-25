using SimCityBuildItBot.Bot;
using System;

namespace SimCityBuildItBot
{
    using Common.Logging.Simple;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Windows.Forms;

    public partial class TradeDepot : Form
    {
        private CaptureScreen captureScreen;
        private Touch touch;
        private TradeWindow tradeWindow;

        private List<PictureBox> pictureBoxes = new List<PictureBox>();
        private List<TextBox> textBoxes = new List<TextBox>();

        public TradeDepot()
        {
            InitializeComponent();
            captureScreen = new CaptureScreen(new NoOpLogger());
            touch = new Touch(new NoOpLogger());
            tradeWindow = new TradeWindow(captureScreen, new LogToText(this.txtLog, this));
            tradeWindow.PictureBox = this.pictureBox1;

            pictureBoxes.Add(this.pb1);
            pictureBoxes.Add(this.pb2);
            pictureBoxes.Add(this.pb3);
            pictureBoxes.Add(this.pb4);
            pictureBoxes.Add(this.pb5);
            pictureBoxes.Add(this.pb6);
            pictureBoxes.Add(this.pb7);
            pictureBoxes.Add(this.pb8);

            textBoxes.Add(this.tb1);
            textBoxes.Add(this.tb2);
            textBoxes.Add(this.tb3);
            textBoxes.Add(this.tb4);
            textBoxes.Add(this.tb5);
            textBoxes.Add(this.tb6);
            textBoxes.Add(this.tb7);
            textBoxes.Add(this.tb8);
        }

        private void btnCaptureImages_Click(object sender, EventArgs e)
        {
            try
            {
                var image = this.tradeWindow.CaptureTradeWindow(captureScreen);

                List<int> tops = GetGlobalTradeTops(image);

                var locs = FindPanelStartsGlobalTrade(image, tops);

                using (var graphics = Graphics.FromImage(image))
                {
                    locs.ForEach(p =>
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            graphics.DrawLine(redPen, p.Start.X, p.Start.Y - 15 + j, p.Start.X + p.Width, p.Start.Y - 15 + j);
                        }
                    });

                    locs.ForEach(p =>
                    {
                        var xx = p.ImageGlobalTradePoint.X;
                        var yy = p.ImageGlobalTradePoint.Y;
                        var size = p.ImageGlobalTradeSize;
                        graphics.DrawPolygon(redPen, new Point[] { p.ImageGlobalTradePoint, new Point(xx + size.Width, yy), new Point(xx + size.Width, yy + size.Height), new Point(xx, yy + size.Height) });
                    });
                }

                ImageViewer.ShowBitmap(image, this.pictureBoxAll);
            }
            catch
            { }
        }

        private List<int> GetGlobalTradeTops(Bitmap image)
        {
            var tops = this.tradeWindow.imageTopsGlobalTrade;

            if (this.cbFindTops.Checked)
            {
                tops = FindTopsOfPanel(image, redRangeGlobalTrade, greenRangeGlobalTrade, blueRangeGlobalTrade);
            }

            return tops;
        }

        private Tuple<byte, byte> redRangeGlobalTrade = new Tuple<byte, byte>(60, 90);
        private Tuple<byte, byte> greenRangeGlobalTrade = new Tuple<byte, byte>(175, 201);
        private Tuple<byte, byte> blueRangeGlobalTrade = new Tuple<byte, byte>(200, 230);

        private Tuple<byte, byte> redRangeTradeDepot = new Tuple<byte, byte>(245, 255);
        private Tuple<byte, byte> greenRangeTradeDepot = new Tuple<byte, byte>(225, 240);
        private Tuple<byte, byte> blueRangeTradeDepot = new Tuple<byte, byte>(185, 210);

        private Pen redPen = new Pen(Color.Red, 2);
        private Pen greenPen = new Pen(Color.Green, 2);

        public static bool InRange(byte value, Tuple<byte, byte> range)
        {
            return value >= range.Item1 && value <= range.Item2;
        }

        private List<PanelLocation> FindPanelStartsTradeDepot(Bitmap image, List<int> tops)
        {
            return FindPanelStarts(image, tops, redRangeTradeDepot, greenRangeTradeDepot, blueRangeTradeDepot);
        }

        private List<PanelLocation> FindPanelStartsGlobalTrade(Bitmap image, List<int> tops)
        {
            return FindPanelStarts(image, tops, redRangeGlobalTrade, greenRangeGlobalTrade, blueRangeGlobalTrade);
        }

        private List<PanelLocation> FindPanelStarts(Bitmap image, List<int> tops, Tuple<byte, byte> redRange, Tuple<byte, byte> greenRange, Tuple<byte, byte> blueRange)
        {
            var result = new List<PanelLocation>();
            int lastFoundX = 0;

            tops.ForEach(y =>
            {
                PanelLocation current = null;

                for (int x = 1; x < image.Width; x++)
                {
                    var p = image.GetPixel(x, y);
                    if (InRange(p.R, redRange) && InRange(p.G, greenRange) && InRange(p.B, blueRange))
                    {
                        lastFoundX = x;

                        if (cbDebug.Checked)
                        {
                            using (var graphics = Graphics.FromImage(image))
                            {
                                graphics.DrawLine(greenPen, x, y, x + 1, y);
                            }
                        }

                        if (current == null)
                        {
                            current = new PanelLocation { Start = new Point(x, y) };
                        }
                    }
                    else
                    {
                        if (current != null)
                        {
                            int width = x - current.Start.X;
                            if (width > 180)
                            {
                                current.Width = width;
                                result.Add(current);
                                lastFoundX = 0;
                                current = null;
                            }
                            else
                            {
                                if (x - lastFoundX > 5)
                                {
                                    current = null;
                                }
                            }
                        }
                    }
                }
            });

            return result;
        }

        private void btnCrop_Click(object sender, EventArgs e)
        {
            string subPath = CreateFolder("CropTest");
            var panels = CaptureImagesGlobalTrade();
            panels.ForEach(panel =>
            {
                ImageViewer.ShowBitmap(panel.CroppedImage, this.pictureBoxAll);
                panel.CroppedImage.Save(subPath + @"\" + Guid.NewGuid().ToString() + ".png", ImageFormat.Png);
            });
        }

        public List<PanelLocation> CaptureImagesGlobalTrade()
        {
            var image = this.tradeWindow.CaptureTradeWindow(captureScreen);

            List<int> tops = GetGlobalTradeTops(image);

            var locs = FindPanelStartsGlobalTrade(image, tops);

            locs.ForEach(p =>
            {
                var crop = cropImage(image, new Rectangle(p.ImageGlobalTradePoint, p.ImageGlobalTradeSize));
                p.CroppedImage = crop;
            });

            return locs;
        }

        private static string CreateFolder(string subPath)
        {
            if (!Directory.Exists(subPath))
            {
                Directory.CreateDirectory(subPath);
            }

            return subPath;
        }

        private static Bitmap cropImage(Bitmap bmpImage, Rectangle cropArea)
        {
            return bmpImage.Clone(cropArea, bmpImage.PixelFormat);
        }

        private void btnSwipe_Click(object sender, EventArgs e)
        {
            touch.Swipe(Bot.Location.GlobalTradeMiddleRight, Bot.Location.GlobalTradeMiddleRight, Bot.Location.GlobalTradeMiddleLeft, swipeSteps, false);
        }

        private long folderNumber = 1;

        private void btnStartCapture_Click(object sender, EventArgs e)
        {
            this.tradeWindow.StopCapture = false;

            string subPath = CreateFolder("Capture");

            Dictionary<ulong, string> hashes = ReadHashes(subPath);

            while (!this.tradeWindow.StopCapture)
            {
                touch.ClickAt(Bot.Location.GlobalTradeRefresh);
                Sleep(1);

                while (CaptureImagesGlobalTrade().Count == 0)
                {
                    this.Text = "Waiting for new images";
                    touch.ClickAt(Bot.Location.GlobalTradeRefresh);
                    Sleep(1);
                }

                var panels = ProcessCaptureImages(subPath, hashes, CaptureImagesGlobalTrade());
                buyIfMatchFound(panels);

                touch.Swipe(Bot.Location.GlobalTradeMiddleRight, Bot.Location.GlobalTradeMiddleRight, Bot.Location.GlobalTradeMiddleLeft, swipeSteps, true);
                Sleep(1);

                panels = ProcessCaptureImages(subPath, hashes, CaptureImagesGlobalTrade());
                buyIfMatchFound(panels);

                touch.Swipe(Bot.Location.GlobalTradeMiddleRight, Bot.Location.GlobalTradeMiddleRight, Bot.Location.GlobalTradeMiddleLeft, swipeSteps, true);
                Sleep(1);

                panels = ProcessCaptureImages(subPath, hashes, CaptureImagesGlobalTrade());
                buyIfMatchFound(panels);
                Sleep(1);

                WaitFor(this.tradeWindow.IsResetButtonVisible, "Reset Button");
            }
        }

        private static Dictionary<ulong, string> ReadHashes(string subPath)
        {
            var directories = Directory.GetDirectories(subPath);

            var hashes = new Dictionary<ulong, string>();

            // get existing folders
            directories.ToList().ForEach(dir =>
            {
                Directory.GetFiles(dir)
                .ToList()
                .ForEach(file =>
                {
                    var hash = file.Substring(file.LastIndexOf(@"\") + 1);
                    hash = hash.Substring(0, hash.Length - 4);
                    hashes.Add(ulong.Parse(hash), dir);
                }
                );
            });
            return hashes;
        }

        private void buyIfMatchFound(List<PanelLocation> panels)
        {
            foreach (var panel in panels)
            {
                if (!string.IsNullOrEmpty(panel.Item))
                {
                    var desired = DesiredItems.Where(d => panel.Item.ToLower().Contains(d)).FirstOrDefault();

                    if (desired != null)
                    {
                        var clickPoint = this.tradeWindow.CalcClickPointGlobalTrade(panel);
                        touch.ClickAt(clickPoint);
                        MessageBox.Show(desired);
                        return;
                    }
                }
            }
        }

        // dozer wheel "planks",
        public List<string> DesiredItems = new List<string> { "camera", "vus", "dozer", "exhaust", "blade", "shipswheel", "lifebelt", "scuba", "lock", "bars", "snow", "winter", "compass" };

        public List<string> BuyItems = new List<string> { "camera","wheel", "vus", "exhaust","bars", "lock", "shipswheel", "lifebelt", "scuba" };

        private void WaitFor(Func<bool> wait, string waitingFor)
        {
            WaitFor(wait, waitingFor, 60);
        }

        private void WaitFor(Func<bool> wait, string waitingFor, long timeoutSecs)
        {
            var text = this.Text;

            var sw = new Stopwatch();
            sw.Start();

            while (!wait() && timeoutSecs > (sw.ElapsedMilliseconds / 1000))
            {
                Application.DoEvents();
                this.Text = text + " - Waitied for" + waitingFor + " for " + ((int)(sw.ElapsedMilliseconds / 1000)) + " seconds";
                Thread.Sleep(500);
            }

            this.Text = text;
        }

        private void Sleep(int seconds)
        {
            var text = this.Text;

            var sw = new Stopwatch();
            sw.Start();

            while (sw.ElapsedMilliseconds < seconds * 1000)
            {
                Application.DoEvents();
                this.Text = text + " - Sleeping for " + (seconds - ((int)(sw.ElapsedMilliseconds / 1000))) + " seconds";
                Thread.Sleep(500);
            }

            this.Text = text;
        }

        private void T_Tick(object sender, EventArgs e)
        {
            Application.DoEvents();
        }

        private List<PanelLocation> ProcessCaptureImages(string subPath, Dictionary<ulong, string> hashes, List<PanelLocation> panels)
        {
            var n = -1;

            this.pictureBoxes.ForEach(p =>
            {
                p.Visible = false;
            });

            this.textBoxes.ForEach(t => t.Text = "");

            panels.ForEach(panel =>
            {
                n++;
                ImageViewer.ShowBitmap(panel.CroppedImage, this.pictureBoxes[n]);
                this.pictureBoxes[n].Visible = true;

                ulong hash = ImageHashing.ImageHashing.AverageHash(panel.CroppedImage);
                if (!hashes.ContainsKey(hash))
                {
                    var bestMatch = hashes.Keys.Where(key => ImageHashing.ImageHashing.Similarity(key, hash) > 90)
                        .OrderByDescending(key => ImageHashing.ImageHashing.Similarity(key, hash))
                        .FirstOrDefault();

                    var folder = string.Empty;

                    if (bestMatch == ulong.MinValue)
                    {
                        // new unknown image
                        folder = subPath + @"\F" + this.folderNumber;

                        // create a folder
                        while (Directory.Exists(folder))
                        {
                            folderNumber++;
                            folder = subPath + @"\F" + this.folderNumber;
                        }

                        CreateFolder(folder);
                    }
                    else
                    {
                        // matched image but a new hash
                        folder = hashes[bestMatch];
                    }

                    // save the image
                    var filename = folder + @"\" + hash + ".png";

                    if (!File.Exists(filename))
                    {
                        panel.CroppedImage.Save(filename, ImageFormat.Png);
                    }

                    // add the hash
                    hashes.Add(hash, folder);

                    this.Text = DateTime.Now.ToLongTimeString() + " - found " + (bestMatch == ulong.MinValue ? "new" : "match") + " " + folder;
                    ImageViewer.ShowBitmap(panel.CroppedImage, this.pictureBoxAll);
                    panel.CroppedImage.Dispose();
                    panel.CroppedImage = null;

                    panel.Item = folder;
                }
                else
                {
                    panel.Item = hashes[hash];
                }

                this.textBoxes[n].Text = panel.ItemText;
            });

            Application.DoEvents();

            return panels;
        }

        private void btnStopCapture_Click(object sender, EventArgs e)
        {
            tradeWindow.StopCapture = true;
        }

        private void btnCaptureResetButton_Click(object sender, EventArgs e)
        {
            this.btnCaptureResetButton.BackColor = this.tradeWindow.IsResetButtonVisible() ? Color.Green : Color.Red;
        }

        private void btnClickPanel_Click(object sender, EventArgs e)
        {
            var image = this.tradeWindow.CaptureTradeWindow(captureScreen);

            List<int> tops = GetGlobalTradeTops(image);

            var locs = FindPanelStartsGlobalTrade(image, tops);

            var i = 0;
            if (int.TryParse(this.cboPanel.SelectedItem.ToString(), out i))
            {
                if (locs.Count > i)
                {
                    var loc = locs[i];
                    var clickPoint = this.tradeWindow.CalcClickPointGlobalTrade(loc);

                    var pointTo = Constants.GetPoint(Bot.Location.GlobalTradeMiddleLeft);

                    touch.Swipe(clickPoint, clickPoint, pointTo, 1, true);
                }
            }
        }

        private void btnCaptureImagesTradeDepot_Click(object sender, EventArgs e)
        {
            try
            {
                var image = this.tradeWindow.CaptureTradeWindow(captureScreen);

                var tops = GetTradeDepotTops(image);

                var locs = FindPanelStartsTradeDepot(image, tops);

                using (var graphics = Graphics.FromImage(image))
                {
                    locs.ForEach(p =>
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            graphics.DrawLine(redPen, p.Start.X, p.Start.Y - 15 + j, p.Start.X + p.Width, p.Start.Y - 15 + j);
                        }
                    });

                    locs.ForEach(p =>
                    {
                        var xx = p.ImageTradeDepotPoint.X;
                        var yy = p.ImageTradeDepotPoint.Y;
                        var size = p.ImageTradeDepotSize;
                        graphics.DrawPolygon(redPen, new Point[] { p.ImageTradeDepotPoint, new Point(xx + size.Width, yy), new Point(xx + size.Width, yy + size.Height), new Point(xx, yy + size.Height) });
                    });
                }

                ImageViewer.ShowBitmap(image, this.pictureBoxAll);
            }
            catch (Exception ex)
            {
                this.Text = ex.Message;
            }
        }

        private List<int> GetTradeDepotTops(Bitmap image)
        {
            var tops = this.tradeWindow.imageTopsTradeDepot;

            if (this.cbFindTops.Checked)
            {
                tops = FindTopsOfPanel(image, redRangeTradeDepot, greenRangeTradeDepot, blueRangeTradeDepot);
            }

            return tops;
        }

        private List<int> FindTopsOfPanel(Bitmap image, Tuple<byte, byte> redRange, Tuple<byte, byte> greenRange, Tuple<byte, byte> blueRange)
        {
            int x1 = 160;
            var top = -1;
            var top2 = -1;

            using (var graphics = Graphics.FromImage(image))
            {
                for (int y = 50; y < 600; y++)
                {
                    var p = image.GetPixel(x1, y);
                    var pen = greenPen;
                    if (InRange(p.R, redRange) && InRange(p.G, greenRange) && InRange(p.B, blueRange))
                    {
                        if (top == -1)
                        {
                            top = y;
                        }
                        else
                        {
                            if (top2 == -1 && y > top + 330)
                            {
                                top2 = y;
                            }
                        }

                        pen = redPen;
                    }

                    graphics.DrawLine(pen, 20, y, 30, y);
                }
            }

            return new List<int> { top, top2 };
        }

        private int swipeSteps = 10;

        private void btnTradeDepotStartCapture_Click(object sender, EventArgs e)
        {
            this.btnTradeDepotStartCapture.Enabled = false;

            Random random = new Random();

            tradeWindow.StopCapture = false;

            string subPath = CreateFolder("Capture");

            Dictionary<ulong, string> hashes = ReadHashes(subPath);

            Action waitForNewImages = () =>
            {
                int n = 0;
                while (CaptureImagesGlobalTrade().Count == 0)
                {
                    n = n + 1;
                    this.Text = "Waiting for new images (" + n + ")";
                    if (this.tradeWindow.IsResetButtonVisible())
                    {
                        touch.ClickAt(Bot.Location.GlobalTradeRefresh);
                    }
                    Sleep(1);

                    if (n == 15)
                    {
                        touch.ClickAt(Bot.Location.GlobalTradeOk);
                        touch.ClickAt(Bot.Location.HomeButton);
                        touch.ClickAt(Bot.Location.HomeButton);
                        touch.ClickAt(Bot.Location.GlobalTrade);
                        n = 0;
                    }
                }
            };

            while (!tradeWindow.StopCapture)
            {
                waitForNewImages();

                PanelLocation desirableItem = ScanGlobalForDesiredItems(subPath, hashes);

                // refresh if no desired item
                if (desirableItem == null && this.tradeWindow.IsResetButtonVisible())
                {
                    touch.ClickAt(Bot.Location.GlobalTradeRefresh);
                    Sleep(1);
                    waitForNewImages();
                    desirableItem = ScanGlobalForDesiredItems(subPath, hashes);
                }

                if (desirableItem != null)
                {
                    var clickPoint = this.tradeWindow.CalcClickPointGlobalTrade(desirableItem);
                    touch.ClickAt(clickPoint);
                }
                else
                {
                    // click random panel
                    var globalPanels = ProcessCaptureImages(subPath, hashes, CaptureImagesGlobalTrade());
                    var clickPoint = this.tradeWindow.CalcClickPointGlobalTrade(globalPanels[random.Next(globalPanels.Count())]);
                    touch.ClickAt(clickPoint);
                }
                Sleep(1);

                // wait for items to appear
                WaitFor(this.tradeWindow.IsTradeDepotLogoVisible, "Trade Logo");
                Sleep(1);
                SleepUntilTradeDepotItemsAreVisible(30);
                var panels = ProcessCaptureImages(subPath, hashes, CaptureImagesTradeDepot());
                BuyTradeDepotItems(panels);

                if (panels.Count() > 7)
                {
                    // swipe
                    touch.Swipe(Bot.Location.GlobalTradeMiddleRight, Bot.Location.GlobalTradeMiddleRight, Bot.Location.GlobalTradeMiddleLeft, swipeSteps, true);
                    Sleep(1);
                    panels = ProcessCaptureImages(subPath, hashes, CaptureImagesTradeDepot());
                    BuyTradeDepotItems(panels);
                }

                if (panels.Count() > 7)
                {
                    touch.Swipe(Bot.Location.GlobalTradeMiddleRight, Bot.Location.GlobalTradeMiddleRight, Bot.Location.GlobalTradeMiddleLeft, swipeSteps, true);
                    Sleep(1);
                    panels = ProcessCaptureImages(subPath, hashes, CaptureImagesTradeDepot());
                    BuyTradeDepotItems(panels);
                }

                // Click home button
                Sleep(1);

                int nn = 0;
                while (this.tradeWindow.IsTradeDepotLogoVisible())
                {
                    nn++;
                    touch.ClickAt(Bot.Location.GlobalTradeOtherCity);
                    if (nn == 60)
                    {
                        touch.ClickAt(Bot.Location.GlobalTradeOk);
                        nn = 0;
                    }
                }
                touch.ClickAt(Bot.Location.GlobalTradeOtherCity);
                Sleep(2);

                if (CaptureImagesGlobalTrade().Count == 0)
                {
                    touch.ClickAt(Bot.Location.HomeButton);
                }
            }

            this.btnTradeDepotStartCapture.Enabled = true;
        }

        private void BuyTradeDepotItems(List<PanelLocation> panels)
        {
            foreach (var item in panels)
            {
                var buyItem = BuyItems.Where(d => item.Item.ToLower().Contains(d)).FirstOrDefault();
                if (buyItem != null)
                {
                    var clickPoint = this.tradeWindow.CalcClickPointTradeDepot(item);
                    touch.ClickAt(clickPoint);
                    Debug.WriteLine(DateTime.Now.ToShortTimeString() + " Bought: " + item.Item);
                    Sleep(4);
                }
            }
        }

        private PanelLocation GetDesiredItem(string subPath, Dictionary<ulong, string> hashes, List<PanelLocation> panels)
        {
            var items = ProcessCaptureImages(subPath, hashes, panels);

            foreach (var desired in DesiredItems)
            {
                var match = items.Where(item => item.Item.ToLower().Contains(desired)).FirstOrDefault();
                if (match != null)
                {
                    return match;
                }
            }

            return null;
        }

        private PanelLocation ScanGlobalForDesiredItems(string subPath, Dictionary<ulong, string> hashes)
        {
            for (int i = 0; i < 3; i++)
            {
                var items = CaptureImagesGlobalTrade();

                var item = GetDesiredItem(subPath, hashes, items);
                if (item != null)
                {
                    return item;
                }

                if (items.Count < 8 && items.Count != 6)
                {
                    return null;
                }

                if (i < 2)
                {
                    touch.Swipe(Bot.Location.GlobalTradeMiddleRight, Bot.Location.GlobalTradeMiddleRight, Bot.Location.GlobalTradeMiddleLeft, swipeSteps, true);
                    Sleep(1);
                }
            }

            return null;
        }

        public List<PanelLocation> CaptureImagesTradeDepot()
        {
            var image = this.tradeWindow.CaptureTradeWindow(captureScreen);

            List<int> tops = GetTradeDepotTops(image);

            var locs = FindPanelStartsTradeDepot(image, tops);

            locs.ForEach(p =>
            {
                p.CroppedImage = cropImage(image, new Rectangle(p.ImageTradeDepotPoint, p.ImageTradeDepotSize));
            });

            return locs;
        }

        private void SleepUntilTradeDepotItemsAreVisible(long timeoutSecs)
        {
            var text = this.Text;

            var sw = new Stopwatch();
            sw.Start();

            while (true)
            {
                if (GetTradeDepotLocations().Count() > 0 || sw.ElapsedMilliseconds > (timeoutSecs * 1000))
                {
                    break;
                }

                Application.DoEvents();
                this.Text = text + " - Waitied for trade depot for " + ((int)(sw.ElapsedMilliseconds / 1000)) + " seconds";
                Thread.Sleep(500);
            }

            this.Text = text;
        }

        private List<PanelLocation> GetTradeDepotLocations()
        {
            var image = this.tradeWindow.CaptureTradeWindow(captureScreen);
            var tops = this.tradeWindow.imageTopsTradeDepot;
            var locs = FindPanelStartsTradeDepot(image, tops);
            return locs;
        }

        private void btnCaptureConfigButton_Click(object sender, EventArgs e)
        {
            this.btnCaptureConfigButton.BackColor = this.tradeWindow.IsConfigButtonVisible() ? Color.Green : Color.Red;
        }

        private void btnCaptureTradeDepot_Click(object sender, EventArgs e)
        {
            this.btnCaptureTradeDepot.BackColor = this.tradeWindow.IsTradeDepotLogoVisible() ? Color.Green : Color.Red;
        }

        private void btnClickPanelTrade_Click(object sender, EventArgs e)
        {
            var image = this.tradeWindow.CaptureTradeWindow(captureScreen);

            List<int> tops = GetTradeDepotTops(image);

            var locs = FindPanelStartsTradeDepot(image, tops);

            var i = 0;
            if (int.TryParse(this.cboPanel.SelectedItem.ToString(), out i))
            {
                if (locs.Count > i)
                {
                    var loc = locs[i];
                    var clickPoint = this.tradeWindow.CalcClickPointTradeDepot(loc);

                    var pointTo = Constants.GetPoint(Bot.Location.GlobalTradeMiddleLeft);

                    touch.Swipe(clickPoint, clickPoint, pointTo, 1, true);
                }
            }
        }
    }
}