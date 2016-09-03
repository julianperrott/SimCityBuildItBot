namespace SimCityBuildItBot
{
    using Common.Logging;
    using SimCityBuildItBot.Bot;
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

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
            salesman = new Salesman(touch, tradeWindow, tradePanelCapture, itemHashes, navigateToBuilding);

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
                    log.Info("Sleeping for 2 mins");
                    Bot.BotApplication.Wait(1000 * 60 * 2); // sleep for 2 mins
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
    }
}