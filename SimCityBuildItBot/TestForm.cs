using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SimCityBuildItBot.Bot;

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

        public TestForm()
        {
            InitializeComponent();
            log = new LogToText(this.txtLog, this);
            touch = new Touch(log);
            buildingSelector = new BuildingSelector(log, touch);
            navigateToBuilding = new NavigateToBuilding(log, touch, buildingSelector);
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
            foreach (BuildingMatch buildingMatch in BuildingMatch.Create())
            {
                if (buildingMatch.Building.ToString() == cboBuilding.SelectedItem.ToString())
                {
                    navigateToBuilding.NavigateTo(buildingMatch, 1);

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
    }
}