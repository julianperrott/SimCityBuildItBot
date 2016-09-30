namespace SimCityBuildItBot
{
    using Common.Logging;
    using SimCityBuildItBot.Bot;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Windows.Forms;
    using static Bot.Salesman;

    public partial class BotForm : Form
    {
        private ILog log;

        private BuildingSelector buildingSelector;
        private NavigateToBuilding navigateToBuilding;
        private Touch touch;
        private CommerceResourceReader resourceReader;
        private List<CommerceItemBuild> buildItemList;
        private TradeWindow tradeWindow;
        private CaptureScreen captureScreen;
        private ItemHashes itemHashes;
        private Salesman salesman;
        private TradePanelCapture tradePanelCapture;
        private Craftsman craftsman;

        public BotForm()
        {
            InitializeComponent();
            log = new LogToText(this.txtLog, this);
            touch = new Touch(log);
            buildingSelector = new BuildingSelector(log, touch);

            captureScreen = new CaptureScreen(log);
            tradeWindow = new TradeWindow(captureScreen, log);

            navigateToBuilding = new NavigateToBuilding(log, touch, buildingSelector, tradeWindow);
            resourceReader = new CommerceResourceReader(log, touch);
            buildItemList = CommerceItemBuild.CreateResourceList();
            tradePanelCapture = new TradePanelCapture(tradeWindow);

            itemHashes = new ItemHashes(new List<PictureBox>(), new List<TextBox>());
            itemHashes.ReadHashes();
            salesman = new Salesman(touch, tradeWindow, tradePanelCapture, itemHashes, navigateToBuilding, log);

            craftsman = new Craftsman(log, buildingSelector, navigateToBuilding, touch, resourceReader, buildItemList);
        }

        private void btnBuildAvailableItems_Click(object sender, EventArgs e)
        {
            while (true)
            {
                bool buildingItems = craftsman.Craft();
                //salesman.Sell();

                if (buildingItems)
                {
                    log.Info("Sleeping for 1 mins");
                    Bot.BotApplication.Wait(1000 * 60 * 1); // sleep for 2 mins
                }
                else
                {
                    log.Info("Sleeping for 4 mins");
                    Bot.BotApplication.Wait(1000 * 60 * 4); // sleep for 4 mins
                    this.txtLog.Text = "";
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new DebugForm().ShowDialog();
        }

        private void btnBuy_Click(object sender, EventArgs e)
        {
            this.Hide();
            new BuyForm().ShowDialog();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();

            Process.GetCurrentProcess().Kill();
        }

        private void btnSellTheCraft_Click(object sender, EventArgs e)
        {
            while (true)
            {
                if (this.IsDisposed)
                {
                    return;
                }

                var sw = new Stopwatch();
                sw.Start();

                bool buildingItems = craftsman.Craft();
                if (this.IsDisposed)
                {
                    return;
                }

                log.Info("Sleeping for 1 mins");
                var millisecond = 60 * 1000;

                while (sw.ElapsedMilliseconds < millisecond)
                {
                    log.Trace("Sleep remaining = " + (int)((millisecond - sw.ElapsedMilliseconds) / 1000) + " seconds");
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(50);
                    if (this.IsDisposed)
                    {
                        return;
                    }
                }

                this.txtLog.Text = "";

                string itemSold;

                salesman.Sell(out itemSold);
                if (this.IsDisposed)
                {
                    return;
                }
            }
        }

        private void L12Craft_Click(object sender, EventArgs e)
        {
            NavigateToBuilding.FactorySwitch = Building.BasicFactory;

            while (true)
            {
                bool buildingItems = craftsman.CraftLevel12();
                //salesman.Sell();

                log.Info("Sleeping for 1 mins");
                Bot.BotApplication.Wait(1000 * 60 * 1); // sleep for 2 mins
                this.txtLog.Text = "";
            }
        }


        private void L12SellAndCraft_Click(object sender, EventArgs e)
        {
            NavigateToBuilding.FactorySwitch = Building.BasicFactory;

            while (true)
            {
                if(this.IsDisposed)
                {
                    return;
                }

                var sw = new Stopwatch();
                sw.Start();

                bool buildingItems = craftsman.CraftLevel12();
                if (this.IsDisposed)
                {
                    return;
                }

                log.Info("Sleeping for 1 mins");
                var millisecond = 60 * 1000;

                while (sw.ElapsedMilliseconds < millisecond)
                {
                    log.Trace("Sleep remaining = " + (int)((millisecond - sw.ElapsedMilliseconds) / 1000) + " seconds");
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(50);
                    if (this.IsDisposed)
                    {
                        return;
                    }
                }

                this.txtLog.Text = "";

                string itemSold;

                salesman.Sell(out itemSold);
                if (this.IsDisposed)
                {
                    return;
                }
            }
        }
    }
}