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

        private List<PictureBox> pictureBoxes = new List<PictureBox>();
        private List<TextBox> textBoxes = new List<TextBox>();

        public TradeDepot()
        {
            InitializeComponent();
            captureScreen = new CaptureScreen(new NoOpLogger());
            touch = new Touch(new NoOpLogger());

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

        public class TradeWindow
        {
            public int X = 287;
            public int Y = 165;
            public Size Size = new Size(1300, 800);

            public List<int> imageTopsGlobalTrade = new List<int>() { 93, 444 };
            public List<int> imageTopsTradeDepot = new List<int>() { 154, 505 };

            public int clickXExtra = 272;
            public List<int> clickY = new List<int>() { 374, 738 };

            public const ulong ResetButtonHash = 17521127647493644;

            public const ulong ConfigButtonHash = 35869570894094686;
            public const ulong TradeDepotLogoHash = 4071112760807423612;

            public Bitmap Capture(CaptureScreen captureScreen)
            {
                return captureScreen.SnapShot("MEmu", X, Y, Size);
            }

            public Bitmap CaptureResetButton(CaptureScreen captureScreen)
            {
                return captureScreen.SnapShot("MEmu", 835, 935, new Size(55, 55));
            }

            public Bitmap CaptureConfigButton(CaptureScreen captureScreen)
            {
                return captureScreen.SnapShot("MEmu", 60, 275, new Size(90, 90));
            }

            public Bitmap CaptureTradeDepotLogo(CaptureScreen captureScreen)
            {
                return captureScreen.SnapShot("MEmu", 320, 95, new Size(60, 65));
            }

            public Point CalcClickPointGlobalTrade(PanelLocation loc)
            {
                var y = loc.Start.Y >= imageTopsGlobalTrade[1] ? clickY[1] : clickY[0];
                var x = clickXExtra + loc.Start.X + 120;

                return new Point(x, y);
            }

            public Point CalcClickPointTradeDepot(PanelLocation loc)
            {
                var y = loc.Start.Y >= imageTopsTradeDepot[1] ? clickY[1] : clickY[0];
                var x = clickXExtra + loc.Start.X + 120;

                return new Point(x, y);
            }
        }

        private void btnCaptureImages_Click(object sender, EventArgs e)
        {
            try
            {
                var image = new TradeWindow().Capture(captureScreen);

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
            var tops = new TradeWindow().imageTopsGlobalTrade;

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

        //private List<int> FindTopOfPanel(Bitmap image)
        //{
        //    int x1 = 100;
        //    var top = -1;
        //    var top2 = -1;

        //    using (var graphics = Graphics.FromImage(image))
        //    {
        //        for (int y = 50; y < 500; y++)
        //        {
        //            var p = image.GetPixel(x1, y);
        //            var pen = blackPen;
        //            if (InRange(p.R, redRangeGlobalTrade) && InRange(p.G, greenRangeGlobalTrade) && InRange(p.B, blueRangeGlobalTrade))
        //            {
        //                if (top == -1)
        //                {
        //                    top = y;
        //                }
        //                else
        //                {
        //                    if (top2 == -1 && y > top + 330)
        //                    {
        //                        top2 = y;
        //                    }
        //                }

        //                pen = redPen;
        //            }

        //            graphics.DrawLine(pen, 20, y, 25, y);
        //        }
        //    }

        //    return new List<int> { top, top2 };
        //}

        public class PanelLocation
        {
            public Point Start { get; set; }

            public Point ImageGlobalTradePoint
            {
                get
                {
                    return new Point(Start.X + 75, Start.Y + 120);
                }
            }

            public Size ImageGlobalTradeSize
            {
                get
                {
                    return new Size(60, 80);
                }
            }

            public Point ImageTradeDepotPoint
            {
                get
                {
                    return new Point(Start.X + 75, Start.Y + 60);
                }
            }

            public Size ImageTradeDepotSize
            {
                get
                {
                    return new Size(60, 80);
                }
            }

            public int Width { get; internal set; }

            public Bitmap CroppedImage { get; set; }

            public string Item { get; set; }

            public string ItemText
            {
                get
                {
                    if (string.IsNullOrEmpty(this.Item))
                    {
                        return string.Empty;
                    }

                    if (!this.Item.Contains(@"\"))
                    {
                        return this.Item;
                    }

                    return this.Item.Substring(this.Item.LastIndexOf(@"\") + 1);
                }
            }
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
            var image = new TradeWindow().Capture(captureScreen);

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
            Func<bool> resetButtonIsVisible = () =>
            {
                Func<Bitmap> getBitmap = () => { return new TradeWindow().CaptureResetButton(captureScreen); };

                bool isResetButtonVisible = IsImageVisible(getBitmap, TradeWindow.ResetButtonHash);
                return isResetButtonVisible;
            };

            this.capture = true;

            string subPath = CreateFolder("Capture");

            Dictionary<ulong, string> hashes = ReadHashes(subPath);

            while (this.capture)
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

                WaitFor(resetButtonIsVisible, "Reset Button");
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
                        var clickPoint = new TradeWindow().CalcClickPointGlobalTrade(panel);
                        touch.ClickAt(clickPoint);
                        MessageBox.Show(desired);
                        return;
                    }
                }
            }
        }

        // dozer wheel
        public List<string> DesiredItems = new List<string> { "vus", "exhaust", "blade", "shipswheel", "lifebelt", "scuba", "lock","bars","camera", "snow", "winter", "compass" };

        public List<string> BuyItems = new List<string> { "vus", "exhaust", "blade","camera", "lock", "bars", "shipswheel", "lifebelt", "scuba" };
    
        private void WaitFor(Func<bool> wait, string waitingFor)
        {
            WaitFor(wait, waitingFor, 600);
        }

        private void WaitFor(Func<bool> wait, string waitingFor, long timeoutSecs)
        {
            var text = this.Text;

            var sw = new Stopwatch();
            sw.Start();

            while (!wait() && timeoutSecs> (sw.ElapsedMilliseconds / 1000))
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

        private bool capture = false;

        private void btnStopCapture_Click(object sender, EventArgs e)
        {
            capture = false;
        }

        private void btnCaptureResetButton_Click(object sender, EventArgs e)
        {
            Func<Bitmap> getBitmap = () => { return new TradeWindow().CaptureResetButton(captureScreen); };

            bool isResetButtonVisible = IsImageVisible(getBitmap, TradeWindow.ResetButtonHash);

            this.btnCaptureResetButton.BackColor = isResetButtonVisible ? Color.Green : Color.Red;
        }

        private bool IsImageVisible(Func<Bitmap> getImage, ulong desiredHash)
        {
            var image = getImage();
            ImageViewer.ShowBitmap(image, this.pictureBox1);
            ulong hash = ImageHashing.ImageHashing.AverageHash(image);

            var isResetButtonVisible = ImageHashing.ImageHashing.Similarity(desiredHash, hash) > 90;
            if (!isResetButtonVisible)
            {
                if (this.cbDebug.Checked)
                {
                    Debug.WriteLine("hash incorrect: " + hash);
                }
            }

            return isResetButtonVisible;
        }

        private void btnClickPanel_Click(object sender, EventArgs e)
        {
            var image = new TradeWindow().Capture(captureScreen);

            List<int> tops = GetGlobalTradeTops(image);

            var locs = FindPanelStartsGlobalTrade(image, tops);

            var i = 0;
            if (int.TryParse(this.cboPanel.SelectedItem.ToString(), out i))
            {
                if (locs.Count > i)
                {
                    var loc = locs[i];
                    var clickPoint = new TradeWindow().CalcClickPointGlobalTrade(loc);

                    var pointTo = Constants.GetPoint(Bot.Location.GlobalTradeMiddleLeft);

                    touch.Swipe(clickPoint, clickPoint, pointTo, 1, true);
                }
            }
        }

        private void btnCaptureImagesTradeDepot_Click(object sender, EventArgs e)
        {
            try
            {
                var image = new TradeWindow().Capture(captureScreen);

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
            var tops = new TradeWindow().imageTopsTradeDepot;

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
            Func<bool> isResetButtonVisible = () =>
            {
                Func<Bitmap> getBitmap = () => { return new TradeWindow().CaptureResetButton(captureScreen); };

                bool isvisible = IsImageVisible(getBitmap, TradeWindow.ResetButtonHash);
                return isvisible;
            };

            Func<bool> configButtonIsVisible = () =>
            {
                Func<Bitmap> getBitmap = () => { return new TradeWindow().CaptureConfigButton(captureScreen); };

                bool isvisible = IsImageVisible(getBitmap, TradeWindow.ConfigButtonHash);
                return isvisible;
            };

            Func<bool> tradeDepotLogoIsVisible = () =>
            {
                Func<Bitmap> getBitmap = () => { return new TradeWindow().CaptureTradeDepotLogo(captureScreen); };

                bool isvisible = IsImageVisible(getBitmap, TradeWindow.TradeDepotLogoHash);
                return isvisible;
            };

            Random random = new Random();

            this.capture = true;

            string subPath = CreateFolder("Capture");

            Dictionary<ulong, string> hashes = ReadHashes(subPath);

            while (this.capture)
            {
                int n = 0;
                while (CaptureImagesGlobalTrade().Count == 0)
                {
                    n = n + 1;
                    this.Text = "Waiting for new images (" + n + ")";
                    if (isResetButtonVisible())
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

                PanelLocation desirableItem = ScanGlobalForDesiredItems(subPath, hashes);

                if (desirableItem != null)
                {
                    var clickPoint = new TradeWindow().CalcClickPointGlobalTrade(desirableItem);
                    touch.ClickAt(clickPoint);
                }
                else
                {
                    // click random panel
                    var globalPanels = ProcessCaptureImages(subPath, hashes, CaptureImagesGlobalTrade());
                    var clickPoint = new TradeWindow().CalcClickPointGlobalTrade(globalPanels[random.Next(globalPanels.Count())]);
                    touch.ClickAt(clickPoint);
                }
                Sleep(1);

                // wait for items to appear
                WaitFor(tradeDepotLogoIsVisible, "Trade Logo");
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
                while (tradeDepotLogoIsVisible())
                {
                    nn++;
                    touch.ClickAt(Bot.Location.GlobalTradeOtherCity);
                    if (nn==60)
                    {
                        touch.ClickAt(Bot.Location.GlobalTradeOk);
                        nn = 0;
                    }
                }
                touch.ClickAt(Bot.Location.GlobalTradeOtherCity);
                Sleep(2);
                if (isResetButtonVisible())
                {
                    touch.ClickAt(Bot.Location.GlobalTradeRefresh);
                    Sleep(1);
                }

                if (CaptureImagesGlobalTrade().Count == 0)
                {
                    touch.ClickAt(Bot.Location.HomeButton);
                }
            }
        }

        private void BuyTradeDepotItems(List<PanelLocation> panels)
        {
            foreach (var item in panels)
            {
                var buyItem = BuyItems.Where(d => item.Item.ToLower().Contains(d)).FirstOrDefault();
                if (buyItem != null)
                {
                    var clickPoint = new TradeWindow().CalcClickPointTradeDepot(item);
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

                touch.Swipe(Bot.Location.GlobalTradeMiddleRight, Bot.Location.GlobalTradeMiddleRight, Bot.Location.GlobalTradeMiddleLeft, swipeSteps, true);
                Sleep(1);
            }

            return null;
        }

        public List<PanelLocation> CaptureImagesTradeDepot()
        {
            var image = new TradeWindow().Capture(captureScreen);

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
            var image = new TradeWindow().Capture(captureScreen);
            var tops = new TradeWindow().imageTopsTradeDepot;
            var locs = FindPanelStartsTradeDepot(image, tops);
            return locs;
        }

        private void btnCaptureConfigButton_Click(object sender, EventArgs e)
        {
            Func<Bitmap> getBitmap = () => { return new TradeWindow().CaptureConfigButton(captureScreen); };

            bool isVisible = IsImageVisible(getBitmap, TradeWindow.ConfigButtonHash);

            this.btnCaptureConfigButton.BackColor = isVisible ? Color.Green : Color.Red;
        }

        private void btnCaptureTradeDepot_Click(object sender, EventArgs e)
        {
            Func<Bitmap> getBitmap = () => { return new TradeWindow().CaptureTradeDepotLogo(captureScreen); };

            bool isVisible = IsImageVisible(getBitmap, TradeWindow.TradeDepotLogoHash);

            this.btnCaptureTradeDepot.BackColor = isVisible ? Color.Green : Color.Red;
        }

        private void btnClickPanelTrade_Click(object sender, EventArgs e)
        {
            var image = new TradeWindow().Capture(captureScreen);

            List<int> tops = GetTradeDepotTops(image);

            var locs = FindPanelStartsTradeDepot(image, tops);

            var i = 0;
            if (int.TryParse(this.cboPanel.SelectedItem.ToString(), out i))
            {
                if (locs.Count > i)
                {
                    var loc = locs[i];
                    var clickPoint = new TradeWindow().CalcClickPointTradeDepot(loc);

                    var pointTo = Constants.GetPoint(Bot.Location.GlobalTradeMiddleLeft);

                    touch.Swipe(clickPoint, clickPoint, pointTo, 1, true);
                }
            }
        }
    }
}