namespace SimCityBuildItBot.Bot
{
    using System;
    using Common.Logging;

    public class NavigateToBuilding
    {
        public static Building FactorySwitch = Building.MassProductionFactory;

        private BuildingSelector buildingSelector;
        private ILog log;
        private readonly Touch touch;
        private TradeWindow tradeWindow;

        public NavigateToBuilding(ILog log, Touch touch, BuildingSelector buildingSelector, TradeWindow tradeWindow)
        {
            this.log = log;
            this.buildingSelector = buildingSelector;
            this.touch = touch;
            this.tradeWindow = tradeWindow;
        }

        public bool NavigateTo(BuildingMatch desiredBuilding, int depth)
        {
            if (depth == 100)
            {
                log.Info("failed to find building: " + desiredBuilding.ToString());
                return false;
            }

            //log.Info("looking for building: " + desiredBuilding.Building.ToString());

            GoHomeIfAtAnotherCity();

            if ((this.tradeWindow.IsTradeDepotLogoVisible() && desiredBuilding.Building == Building.TradeDepot)
                || (this.tradeWindow.IsGlobalTradeVisible() && desiredBuilding.Building == Building.GlobalTrade)
                )
            {
                log.Info("at desired building: " + desiredBuilding.Building.ToString());
                return true;
            }

            CloseOfflineHomeIfOpen();

            CloseTradeIfOpen();

            if (desiredBuilding.Building == Building.TradeDepot)
            {
                if (!NavigateTo(BuildingMatch.Get(Building.HardwareStore), 1))
                {
                    return false;
                }

                touch.ClickAt(Bot.Location.HomeTradeDepot);

                return NavigateTo(desiredBuilding, depth++);
            }

            if (desiredBuilding.Building == Building.GlobalTrade)
            {
                if (!NavigateTo(BuildingMatch.Get(Building.FastFoodRestaurant), 1))
                {
                    return false;
                }

                touch.ClickAt(Bot.Location.GlobalTradeFromFastFood);

                return NavigateTo(desiredBuilding, depth++);
            }

            var buildingFound = buildingSelector.SelectABuilding(" going to [" + desiredBuilding.Building.ToString()+"]");
            if (buildingFound == null)
            {
                log.Info("failed to find building: " + desiredBuilding.Building.ToString());
                return false;
            }

            // did we find it
            if (buildingFound.Building == desiredBuilding.Building)
            {
                log.Info("at desired building: " + desiredBuilding.Building.ToString());
                return true;
            }

            // do we need to switch building types
            if ((buildingFound.Building == NavigateToBuilding.FactorySwitch || buildingFound.Building == Building.HardwareStore)
            && buildingFound.BuildingType != desiredBuilding.BuildingType
            )
            {
                touch.ClickAt(Location.BelowCentreMap);
            }
            else
            {
                touch.ClickAt(Location.RightButton);
            }

            return NavigateTo(desiredBuilding, depth + 1);
        }

        private void CloseOfflineHomeIfOpen()
        {
            if (this.tradeWindow.IsOfflineButtonVisible())
            {
                Bot.BotApplication.Wait(2000);
                if (this.tradeWindow.IsOfflineButtonVisible())
                {
                    // close it
                    touch.ClickAt(Location.GlobalTradeOk);
                }
            }
        }

        private void CloseTradeIfOpen()
        {
            if (this.tradeWindow.IsEditSaleCloseButtonVisible())
            {
                // close it
                touch.ClickAt(Location.HomeButton);
                touch.ClickAt(Location.HomeButton);
            }

            // is the trade window visible
            if (this.tradeWindow.IsTradeDepotLogoVisible())
            {
                // close it
                touch.ClickAt(Location.TradeDepotClose);
            }

            if (this.tradeWindow.IsGlobalTradeVisible())
            {
                // close it
                touch.ClickAt(Location.GlobalTradeDepotClose);
            }
        }

        private void GoHomeIfAtAnotherCity()
        {
            while (this.tradeWindow.IsHomeButtonVisible())
            {
                for (int i = 0; i < 10; i++)
                {
                    log.Info("at another city, trying to go home "+ (10-i));

                    // we are at another city
                    touch.ClickAt(Bot.Location.HomeButton);

                    if (! this.tradeWindow.IsHomeButtonVisible())
                    {
                        break;
                    }

                    if (i==5) // try to handle global trade dialog
                    {
                        touch.ClickAt(Bot.Location.GlobalTradeOk);
                    }
                }

                if (!this.tradeWindow.IsHomeButtonVisible())
                {
                    // wait to get home
                    log.Info("waiting for home to appear");
                    while (!this.tradeWindow.IsConfigButtonVisible())
                    {
                        BotApplication.Wait(1000);
                    }
                    BotApplication.Wait(5000);
                }
            }
        }
    }
}