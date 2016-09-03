using Common.Logging;
using SimCityBuildItBot.Bot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SimCityBuildItBot
{
    public partial class TestForm : Form
    {
        private ILog log;

        private BuildingSelector buildingSelector;
        private NavigateToBuilding navigateToBuilding;
        private Touch touch;
        private CommerceResourceReader resourceReader;
        private List<CommerceItemBuild> buildItemList;
        private TradeWindow tradeWindow;
        private CaptureScreen captureScreen;

        public TestForm()
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
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            buildingSelector.SelectABuilding();
        }

        private void TestForm_Load(object sender, EventArgs e)
        {
            cboBuilding.Items.AddRange(Enum.GetNames(typeof(Building)));
        }

        private void leftClick_Click(object sender, EventArgs e)
        {
            touch.ClickAt(Bot.Location.LeftButton);
        }

        private void rightClick_Click(object sender, EventArgs e)
        {
            touch.ClickAt(Bot.Location.RightButton);
        }

        private void centreClick_Click(object sender, EventArgs e)
        {
            touch.ClickAt(Bot.Location.CentreMap);
        }

        private void belowClick_Click(object sender, EventArgs e)
        {
            touch.ClickAt(Bot.Location.BelowCentreMap);
        }

        private void btnNavigateTo_Click(object sender, EventArgs e)
        {
            this.btnNavigateTo.BackColor = Color.YellowGreen;

            foreach (BuildingMatch buildingMatch in BuildingMatch.Create())
            {
                if (buildingMatch.Building.ToString() == cboBuilding.SelectedItem.ToString())
                {
                    this.btnNavigateTo.BackColor = navigateToBuilding.NavigateTo(buildingMatch, 1)? Color.Green : Color.Red;

                    // pick up any items
                    touch.ClickAt(Bot.Location.CentreMap);

                    // reselect
                    navigateToBuilding.NavigateTo(buildingMatch, 1);
                }
            }
        }

        private void production1_Click(object sender, EventArgs e)
        {
            touch.ClickAt(Bot.Location.ButtonLeftInner1);
        }

        private void buildItem1_Click(object sender, EventArgs e)
        {
            touch.Swipe(Bot.Location.ButtonLeftInner1, Bot.Location.FactoryQueuePositionStart, Bot.Location.FactoryQueuePositionend, 10, false);
        }

        private void commerceBuild1_Click(object sender, EventArgs e)
        {
            touch.Swipe(Bot.Location.ButtonLeftInner1, Bot.Location.ButtonLeftInner1, Bot.Location.ProductionQueue, 4, true);
        }

        private void production2Woodlog_Click(object sender, EventArgs e)
        {
            touch.ClickAt(Bot.Location.ButtonLeftInner2);
        }

        private void production3Seed_Click(object sender, EventArgs e)
        {
            touch.ClickAt(Bot.Location.ButtonRightInner1);
        }

        private void buildItem2_Click(object sender, EventArgs e)
        {
            touch.Swipe(Bot.Location.ButtonLeftInner2, Bot.Location.FactoryQueuePositionStart, Bot.Location.FactoryQueuePositionend, 10, false);
        }

        private void buildItem3_Click(object sender, EventArgs e)
        {
            touch.Swipe(Bot.Location.ButtonRightInner1, Bot.Location.FactoryQueuePositionStart, Bot.Location.FactoryQueuePositionend, 10, false);
        }

        private void production1Textiles_Click(object sender, EventArgs e)
        {
            touch.ClickAt(Bot.Location.ButtonLeftOuter1);
        }

        private void buildTextiles_Click(object sender, EventArgs e)
        {
            touch.Swipe(Bot.Location.ButtonLeftOuter1, Bot.Location.FactoryQueuePositionStart, Bot.Location.FactoryQueuePositionend, 10, false);
        }

        private void btnNavigateToTradeDepot_Click(object sender, EventArgs e)
        {
            this.btnNavigateToTradeDepot.BackColor = Color.YellowGreen;

            navigateToBuilding.NavigateTo(BuildingMatch.Get(Building.TradeDepot), 1);

            this.btnNavigateToTradeDepot.BackColor = this.tradeWindow.IsTradeDepotLogoVisible() ? Color.Green : Color.Red;
        }

        private void btnNavigateToGlobalTradeDepot_Click(object sender, EventArgs e)
        {
            this.btnNavigateToGlobalTradeDepot.BackColor = Color.YellowGreen;

            navigateToBuilding.NavigateTo(BuildingMatch.Get(Building.GlobalTrade), 1);

            this.btnNavigateToGlobalTradeDepot.BackColor = this.tradeWindow.IsGlobalTradeVisible() ? Color.Green : Color.Red;
        }
    }
}