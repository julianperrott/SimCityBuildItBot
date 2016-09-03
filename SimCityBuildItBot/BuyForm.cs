using SimCityBuildItBot.Bot;
using System;

namespace SimCityBuildItBot
{
    using Common.Logging;
    using Common.Logging.Simple;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;

    public partial class BuyForm : Form
    {
        private CaptureScreen captureScreen;
        private Touch touch;
        private TradeWindow tradeWindow;
        private TradePanelCapture tradePanelCapture;
        private ItemHashes itemHashes;

        private BuildingSelector buildingSelector;
        private NavigateToBuilding navigateToBuilding;
        private CommerceResourceReader resourceReader;
        private List<CommerceItemBuild> buildItemList;
        private Salesman salesman;
        private Craftsman craftsman;
        private ILog log;

        public BuyForm()
        {
            InitializeComponent();

            log = new NoOpLogger();

            captureScreen = new CaptureScreen(new NoOpLogger());
            touch = new Touch(log);
            resourceReader = new CommerceResourceReader(log, touch);
            buildingSelector = new BuildingSelector(log, touch);
            tradeWindow = new TradeWindow(captureScreen, log);
            tradePanelCapture = new TradePanelCapture(tradeWindow);
            navigateToBuilding = new NavigateToBuilding(log, touch, buildingSelector, tradeWindow);
            buildItemList = CommerceItemBuild.CreateResourceList();

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

            LoadShoppingLists();

            salesman = new Salesman(touch, tradeWindow, tradePanelCapture, itemHashes, navigateToBuilding);
            craftsman = new Craftsman(log, buildingSelector, navigateToBuilding, touch, resourceReader, buildItemList);
        }

        private void LoadShoppingLists()
        {
            listBoxShoppingList.Items.Clear();
            ShoppingListsForm.GetShoppingLists().ForEach(file =>
            {
                listBoxShoppingList.Items.Add(file.Substring(file.LastIndexOf(@"\") + 1));
            });

            if (listBoxShoppingList.Items.Count > 0)
            {
                listBoxShoppingList.SelectedIndex = 0;
                btnVisitSetList_Click(null, null);
                btnBuySetList_Click(null, null);
            }
        }

        public List<string> GetDesiredItems()
        {
            var items = this.txtVisit.Text.Split(Environment.NewLine.ToCharArray())
                .Where(s => s.Length > 2)
                .ToList();
            return items;
        }

        public List<string> GetBuyItems()
        {
            var items = this.txtBuy.Text.Split(Environment.NewLine.ToCharArray())
                .Where(s => s.Length > 2)
                .ToList();

            return items;
        }

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
                Bot.BotApplication.Wait(500);
            }

            this.Text = text;
        }

        private int swipeSteps = 10;

        private Action waitForNewImages
        {
            get
            {
                return () =>
                {
                    int n = 0;
                    while (tradePanelCapture.CaptureImagesGlobalTrade().Count == 0)
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
            }
        }

        private Random random = new Random();

        private void btnTradeDepotStartCapture_Click(object sender, EventArgs e)
        {
            this.btnTradeDepotStartCapture.Enabled = false;

            tradeWindow.StopCapture = false;

            itemHashes.ReadHashes();

            while (!tradeWindow.StopCapture)
            {
                BuyItemsFromGlobalTrade();
            }

            this.btnTradeDepotStartCapture.Enabled = true;
        }

        private void BuyItemsFromGlobalTrade()
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
                var globalPanels = itemHashes.ProcessCaptureImages(tradePanelCapture.CaptureImagesGlobalTrade());
                if (globalPanels.Count > 0)
                {
                    var clickPoint = this.tradeWindow.CalcClickPointGlobalTrade(globalPanels[random.Next(globalPanels.Count())]);
                    touch.ClickAt(clickPoint);
                }
            }
            WaitForSeconds(1);

            // wait for items to appear
            WaitFor(this.tradeWindow.IsTradeDepotLogoVisible, "Trade Logo");
            WaitForSeconds(1);
            SleepUntilTradeDepotItemsAreVisible(30);
            var panels = itemHashes.ProcessCaptureImages(tradePanelCapture.CaptureImagesTradeDepot());
            BuyTradeDepotItems(panels);

            if (panels.Count() > 7)
            {
                // swipe
                touch.Swipe(Bot.Location.GlobalTradeMiddleRight, Bot.Location.GlobalTradeMiddleRight, Bot.Location.GlobalTradeMiddleLeft, swipeSteps, true);
                WaitForSeconds(1);
                panels = itemHashes.ProcessCaptureImages(tradePanelCapture.CaptureImagesTradeDepot());
                BuyTradeDepotItems(panels);
            }

            if (panels.Count() > 7)
            {
                touch.Swipe(Bot.Location.GlobalTradeMiddleRight, Bot.Location.GlobalTradeMiddleRight, Bot.Location.GlobalTradeMiddleLeft, swipeSteps, true);
                WaitForSeconds(1);
                panels = itemHashes.ProcessCaptureImages(tradePanelCapture.CaptureImagesTradeDepot());
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

            if (this.tradeWindow.IsOfflineButtonVisible())
            {
                // close it
                touch.ClickAt(Bot.Location.GlobalTradeOk);
            }

            WaitForSeconds(2);

            if (tradePanelCapture.CaptureImagesGlobalTrade().Count == 0)
            {
                touch.ClickAt(Bot.Location.HomeButton);
            }
        }

        private void BuyTradeDepotItems(List<PanelLocation> panels)
        {
            foreach (var item in panels)
            {
                var buyItem = GetBuyItems().Where(d => item.Item.ToLower().Contains(d.ToLower())).FirstOrDefault();
                if (buyItem != null)
                {
                    var clickPoint = this.tradeWindow.CalcClickPointTradeDepot(item);
                    touch.ClickAt(clickPoint);
                    listBought.Items.Add(DateTime.Now.ToShortTimeString() + " Bought: " + item.Item.Substring(item.Item.LastIndexOf(@"\") + 1));
                    Debug.WriteLine(DateTime.Now.ToShortTimeString() + " Bought: " + item.Item);
                    WaitForSeconds(4);
                }
            }
        }

        private PanelLocation GetDesiredItem(List<PanelLocation> panels)
        {
            var items = itemHashes.ProcessCaptureImages(panels);

            foreach (var desired in GetDesiredItems())
            {
                var match = items.Where(item => item.Item.ToLower().Contains(desired.ToLower())).FirstOrDefault();
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
                var items = tradePanelCapture.CaptureImagesGlobalTrade();

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

        private void btnDebug_Click(object sender, EventArgs e)
        {
            new TradeDepot().ShowDialog();
        }

        private void listBoxShoppingList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItems = GetItemsFromShoppingList();

            this.listBoxSelected.Items.Clear();
            selectedItems.ForEach(item => this.listBoxSelected.Items.Add(item));
        }

        private List<string> GetItemsFromShoppingList()
        {
            var filename = this.listBoxShoppingList.SelectedItem.ToString();
            var filePath = ShoppingListsForm.path + @"\" + filename;

            var selectedItems = File.ReadAllLines(filePath).ToList();
            return selectedItems;
        }

        private void btnShoppingList_Click(object sender, EventArgs e)
        {
            new ShoppingListsForm().ShowDialog();
            LoadShoppingLists();
        }
        private void btnVisitSetList_Click(object sender, EventArgs e)
        {
            var selectedItems = GetItemsFromShoppingList();
            this.txtVisit.Text = string.Join(Environment.NewLine, selectedItems);
        }

        private void btnBuySetList_Click(object sender, EventArgs e)
        {
            var selectedItems = GetItemsFromShoppingList();
            this.txtBuy.Text = string.Join(Environment.NewLine, selectedItems);
        }

        private void btnBuyItemsAndCraft_Click(object sender, EventArgs e)
        {
            this.btnTradeDepotStartCapture.Enabled = false;

            tradeWindow.StopCapture = false;

            itemHashes.ReadHashes();

            while (!tradeWindow.StopCapture)
            {
                bool buildingItems = craftsman.Craft();
                salesman.Sell();

                var buyTimeAvailableMilliseconds = (buildingItems ? 120 : 240) * 1000;

                var sw = new Stopwatch();
                sw.Start();

                // go to global trade
                bool result = navigateToBuilding.NavigateTo(BuildingMatch.Get(Building.GlobalTrade), 1);

                if (result)
                {
                    while (sw.ElapsedMilliseconds < buyTimeAvailableMilliseconds)
                    {
                        Application.DoEvents();
                        BuyItemsFromGlobalTrade();
                    }
                }
                else
                {
                }
            }

            this.btnTradeDepotStartCapture.Enabled = true;
        }
    }
}