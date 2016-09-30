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
        private TradePanelCapture tradePanelCapture;
        private ItemHashes itemHashes;
        private NavigateToBuilding navigateToBuilding;
        private BuildingSelector buildingSelector;
        private Salesman salesman;

        private static Pen purplePen = new Pen(Color.Purple, 1);
        private static Pen redPen = new Pen(Color.Red, 2);
        private static Pen greenPen = new Pen(Color.Green, 2);

        public TradeDepot()
        {
            InitializeComponent();
            var log = new LogToText(this.txtLog, this);

            captureScreen = new CaptureScreen(log);
            touch = new Touch(log);
            tradeWindow = new TradeWindow(captureScreen, log);
            tradePanelCapture = new TradePanelCapture(tradeWindow);
            buildingSelector = new BuildingSelector(log, touch);
            navigateToBuilding = new NavigateToBuilding(log, touch, buildingSelector, tradeWindow);

            tradeWindow.PictureBox = this.pictureBox1;

            var pictureBoxes = new List<PictureBox>();
            var textBoxes = new List<TextBox>();

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

            itemHashes = new ItemHashes(pictureBoxes, textBoxes);
            itemHashes.ReadHashes();
            salesman = new Salesman(touch, tradeWindow, tradePanelCapture, itemHashes, navigateToBuilding, log);
        }

        private void btnCaptureImages_Click(object sender, EventArgs e)
        {
            try
            {
                var image = this.tradeWindow.CaptureTradeWindow();

                List<int> tops = GetGlobalTradeTops(image);

                var locs = tradePanelCapture.FindPanelStartsGlobalTrade(image, tops);

                DrawDebugLines(image, locs);

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
                tops = tradePanelCapture.FindTopsOfPanelGlobalTrade(image);
            }

            return tops;
        }

        private void btnCrop_Click(object sender, EventArgs e)
        {
            var path = "CropTest";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var panels = CaptureImagesGlobalTrade();
            panels.ForEach(panel =>
            {
                ImageViewer.ShowBitmap(panel.CroppedImage, this.pictureBoxAll);
                panel.CroppedImage.Save(path + @"\" + Guid.NewGuid().ToString() + ".png", ImageFormat.Png);
            });
        }

        public List<PanelLocation> CaptureImagesGlobalTrade()
        {
            var image = this.tradeWindow.CaptureTradeWindow();

            List<int> tops = GetGlobalTradeTops(image);

            var locs = tradePanelCapture.FindPanelStartsGlobalTrade(image, tops);

            locs.ForEach(p =>
            {
                var crop = cropImage(image, new Rectangle(p.ImageGlobalTradePoint, p.ImageGlobalTradeSize));
                p.CroppedImage = crop;
            });

            return locs;
        }

        private static Bitmap cropImage(Bitmap bmpImage, Rectangle cropArea)
        {
            return bmpImage.Clone(cropArea, bmpImage.PixelFormat);
        }

        private void btnSwipe_Click(object sender, EventArgs e)
        {
            touch.Swipe(Bot.Location.GlobalTradeMiddleRight, Bot.Location.GlobalTradeMiddleRight, Bot.Location.GlobalTradeMiddleLeft, swipeSteps, false);
        }

        private void btnStartCapture_Click(object sender, EventArgs e)
        {
            this.tradeWindow.StopCapture = false;

            itemHashes.ReadHashes();

            while (!this.tradeWindow.StopCapture)
            {
                touch.ClickAt(Bot.Location.GlobalTradeRefresh);
                WaitForSeconds(1);

                while (CaptureImagesGlobalTrade().Count == 0)
                {
                    this.Text = "Waiting for new images";
                    touch.ClickAt(Bot.Location.GlobalTradeRefresh);
                    WaitForSeconds(1);
                }

                var panels = itemHashes.ProcessCaptureImages(CaptureImagesGlobalTrade());
                buyIfMatchFound(panels);

                touch.Swipe(Bot.Location.GlobalTradeMiddleRight, Bot.Location.GlobalTradeMiddleRight, Bot.Location.GlobalTradeMiddleLeft, swipeSteps, true);
                WaitForSeconds(1);

                panels = itemHashes.ProcessCaptureImages(CaptureImagesGlobalTrade());
                buyIfMatchFound(panels);

                touch.Swipe(Bot.Location.GlobalTradeMiddleRight, Bot.Location.GlobalTradeMiddleRight, Bot.Location.GlobalTradeMiddleLeft, swipeSteps, true);
                WaitForSeconds(1);

                panels = itemHashes.ProcessCaptureImages(CaptureImagesGlobalTrade());
                buyIfMatchFound(panels);
                WaitForSeconds(1);

                WaitFor(this.tradeWindow.IsRefreshGlobalTradeButtonVisible, "Reset Button");
            }
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

        public List<string> BuyItems = new List<string> { "camera", "wheel", "vus", "exhaust", "bars", "lock", "shipswheel", "lifebelt", "scuba" };

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
                Bot.BotApplication.Wait(500);
            }

            this.Text = text;
        }

        private void WaitForSeconds(int seconds)
        {
            var text = this.Text;

            var sw = new Stopwatch();
            sw.Start();

            while (sw.ElapsedMilliseconds < seconds * 1000)
            {
                Application.DoEvents();
                this.Text = text + " - Sleeping for " + (seconds - ((int)(sw.ElapsedMilliseconds / 1000))) + " seconds";
                Bot.BotApplication.Wait(100);
            }

            this.Text = text;
        }

        private void T_Tick(object sender, EventArgs e)
        {
            Application.DoEvents();
        }

        private void btnStopCapture_Click(object sender, EventArgs e)
        {
            tradeWindow.StopCapture = true;
        }

        private void btnCaptureResetButton_Click(object sender, EventArgs e)
        {
            this.btnCaptureResetButton.BackColor = this.tradeWindow.IsRefreshGlobalTradeButtonVisible() ? Color.Green : Color.Red;
        }

        private void btnClickPanel_Click(object sender, EventArgs e)
        {
            var image = this.tradeWindow.CaptureTradeWindow();

            List<int> tops = GetGlobalTradeTops(image);

            var locs = tradePanelCapture.FindPanelStartsGlobalTrade(image, tops);

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
            itemHashes.ReadHashes();

            try
            {
                var image = this.tradeWindow.CaptureTradeWindow();

                var tops = GetTradeDepotTops(image);

                var locs = tradePanelCapture.FindPanelStartsTradeDepot(image, tops);

                DrawDebugLines(image, locs);

                ImageViewer.ShowBitmap(image, this.pictureBoxAll);

                var panels = itemHashes.ProcessCaptureImages(CaptureImagesTradeDepot());
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
                tops = tradePanelCapture.FindTopsOfPanelTradeDepot(image);
            }

            return tops;
        }

        private int swipeSteps = 10;

        private void btnTradeDepotStartCapture_Click(object sender, EventArgs e)
        {
            this.btnTradeDepotStartCapture.Enabled = false;

            Random random = new Random();

            tradeWindow.StopCapture = false;

            itemHashes.ReadHashes();

            Action waitForNewImages = () =>
            {
                int n = 0;
                while (CaptureImagesGlobalTrade().Count == 0)
                {
                    n = n + 1;
                    this.Text = "Waiting for new images (" + n + ")";
                    if (this.tradeWindow.IsRefreshGlobalTradeButtonVisible())
                    {
                        touch.ClickAt(Bot.Location.GlobalTradeRefresh);
                    }
                    WaitForSeconds(1);

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

                PanelLocation desirableItem = ScanGlobalForDesiredItems();

                // refresh if no desired item
                if (desirableItem == null && this.tradeWindow.IsRefreshGlobalTradeButtonVisible())
                {
                    touch.ClickAt(Bot.Location.GlobalTradeRefresh);
                    WaitForSeconds(1);
                    waitForNewImages();
                    desirableItem = ScanGlobalForDesiredItems();
                }

                if (desirableItem != null)
                {
                    var clickPoint = this.tradeWindow.CalcClickPointGlobalTrade(desirableItem);
                    touch.ClickAt(clickPoint);
                }
                else
                {
                    // click random panel
                    var globalPanels = itemHashes.ProcessCaptureImages(CaptureImagesGlobalTrade());
                    var clickPoint = this.tradeWindow.CalcClickPointGlobalTrade(globalPanels[random.Next(globalPanels.Count())]);
                    touch.ClickAt(clickPoint);
                }
                WaitForSeconds(1);

                // wait for items to appear
                WaitFor(this.tradeWindow.IsTradeDepotLogoVisible, "Trade Logo");
                WaitForSeconds(1);
                SleepUntilTradeDepotItemsAreVisible(30);
                var panels = itemHashes.ProcessCaptureImages(CaptureImagesTradeDepot());
                BuyTradeDepotItems(panels);

                if (panels.Count() > 7)
                {
                    // swipe
                    touch.Swipe(Bot.Location.GlobalTradeMiddleRight, Bot.Location.GlobalTradeMiddleRight, Bot.Location.GlobalTradeMiddleLeft, swipeSteps, true);
                    WaitForSeconds(1);
                    panels = itemHashes.ProcessCaptureImages(CaptureImagesTradeDepot());
                    BuyTradeDepotItems(panels);
                }

                if (panels.Count() > 7)
                {
                    touch.Swipe(Bot.Location.GlobalTradeMiddleRight, Bot.Location.GlobalTradeMiddleRight, Bot.Location.GlobalTradeMiddleLeft, swipeSteps, true);
                    WaitForSeconds(1);
                    panels = itemHashes.ProcessCaptureImages(CaptureImagesTradeDepot());
                    BuyTradeDepotItems(panels);
                }

                // Click home button
                WaitForSeconds(1);

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
                WaitForSeconds(2);

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
                    WaitForSeconds(4);
                }
            }
        }

        private PanelLocation GetDesiredItem(List<PanelLocation> panels)
        {
            var items = itemHashes.ProcessCaptureImages(panels);

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

        private PanelLocation ScanGlobalForDesiredItems()
        {
            for (int i = 0; i < 3; i++)
            {
                var items = CaptureImagesGlobalTrade();

                var item = GetDesiredItem(items);
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
                    WaitForSeconds(1);
                }
            }

            return null;
        }

        public List<PanelLocation> CaptureImagesTradeDepot()
        {
            var image = this.tradeWindow.CaptureTradeWindow();

            List<int> tops = GetTradeDepotTops(image);

            var locs = tradePanelCapture.FindPanelStartsTradeDepot(image, tops);

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
                Bot.BotApplication.Wait(500);
            }

            this.Text = text;
        }

        private List<PanelLocation> GetTradeDepotLocations()
        {
            var image = this.tradeWindow.CaptureTradeWindow();
            var tops = this.tradeWindow.imageTopsTradeDepot;
            var locs = tradePanelCapture.FindPanelStartsTradeDepot(image, tops);
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

        private void btnCaptureEditSale_Click(object sender, EventArgs e)
        {
            this.btnCaptureEditSale.BackColor = this.tradeWindow.IsEditSaleCloseButtonVisible() ? Color.Green : Color.Red;
        }

        private void btnClickPanelTrade_Click(object sender, EventArgs e)
        {
            var image = this.tradeWindow.CaptureTradeWindow();

            List<int> tops = GetTradeDepotTops(image);

            var locs = tradePanelCapture.FindPanelStartsTradeDepot(image, tops);

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

        private void bntCollectSales_Click(object sender, EventArgs e)
        {
            this.bntCollectSales.BackColor = Color.YellowGreen;

            navigateToBuilding.NavigateTo(BuildingMatch.Get(Building.TradeDepot), 1);

            if (!this.tradeWindow.IsTradeDepotLogoVisible())
            {
                this.bntCollectSales.BackColor = Color.Red;
                return;
            }

            btnCaptureImagesTradeDepot_Click(sender, e);
            salesman.CollectSales();

            this.bntCollectSales.BackColor = Color.Gray;
        }


        private void btnMakeSale_Click(object sender, EventArgs e)
        {
            string itemSold;

            this.btnMakeSale.BackColor = Color.YellowGreen;
            Application.DoEvents();

            this.btnMakeSale.BackColor = salesman.Sell(out itemSold) == Salesman.SaleResult.SoldItem ? Color.Green : Color.Red;
        }

      

        private void btnCaptureInventory_Click(object sender, EventArgs e)
        {
            var image = this.tradeWindow.CaptureForSaleInventory();


            var locs = this.tradePanelCapture.CaptureCreateSaleItems();

            using (var graphics = Graphics.FromImage(image))
            {
                locs.ForEach(p =>
                {
                    var size = p.ImageGlobalTradeSize;
                    graphics.DrawPolygon(redPen, new Point[] { p.Start, new Point(p.Start.X + size.Width, p.Start.Y), new Point(p.Start.X + size.Width, p.Start.Y + size.Height), new Point(p.Start.X, p.Start.Y + size.Height) });
                });
            }

            ImageViewer.ShowBitmap(image, this.pictureBoxAll);
            var panels = itemHashes.ProcessCaptureImages(locs);
        }

        private static void DrawDebugLines(Bitmap image, List<PanelLocation> locs)
        {
            using (var graphics = Graphics.FromImage(image))
            {
                var drawOnLine = true;

                locs.ForEach(p =>
                {
                    for (int j = 0; j < 5; j++)
                    {
                        graphics.DrawLine(redPen, p.Start.X, p.Start.Y - 15 + j, p.Start.X + p.Width, p.Start.Y - 15 + j);
                    }

                    if (drawOnLine)
                    {
                        graphics.DrawLine(purplePen, p.Start.X, p.Start.Y, p.Start.X + p.Width, p.Start.Y);
                        drawOnLine = false;
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
        }

        private void btnCaptureGlobalTrade_Click(object sender, EventArgs e)
        {
            this.btnCaptureGlobalTrade.BackColor = this.tradeWindow.IsGlobalTradeVisible() ? Color.Green : Color.Red;
        }

        private void btnIsHomeButtonVisible_Click(object sender, EventArgs e)
        {
            this.btnIsHomeButtonVisible.BackColor = this.tradeWindow.IsHomeButtonVisible() ? Color.Green : Color.Red;
        }

        private void btnIsOfflineButtonVisible_Click(object sender, EventArgs e)
        {
            this.btnIsOfflineButtonVisible.BackColor = this.tradeWindow.IsOfflineButtonVisible() ? Color.Green : Color.Red;
            

        }

        Point pictureBoxAllLocation;

        private void btnToggle_Click(object sender, EventArgs e)
        {
            if (pictureBoxAll.Location.Y>0)
            {
                pictureBoxAllLocation = pictureBoxAll.Location;
                pictureBoxAll.Location = new Point(0, 0);
                pictureBoxAll.BringToFront();
                pictureBoxAll.Height = this.Height;
                btnToggle.BringToFront();
            }
            else
            {
                pictureBoxAll.Location = pictureBoxAllLocation;
            }
        }
    }
}