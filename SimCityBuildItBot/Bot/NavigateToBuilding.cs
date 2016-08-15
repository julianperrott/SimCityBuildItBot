namespace SimCityBuildItBot.Bot
{
    using Common.Logging;

    public class NavigateToBuilding
    {
        private BuildingSelector buildingSelector;
        private ILog log;
        private readonly Touch touch;

        public NavigateToBuilding(ILog log, Touch touch, BuildingSelector buildingSelector)
        {
            this.log = log;
            this.buildingSelector = buildingSelector;
            this.touch = touch;
        }

        public bool NavigateTo(BuildingMatch desiredBuilding, int depth)
        {
            if (depth == 100)
            {
                log.Info("failed to find building: " + desiredBuilding.ToString());
                return false;
            }

            log.Info("looking for building: " + desiredBuilding.Building.ToString());
            var buildingFound = buildingSelector.SelectABuilding();
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
            if ((buildingFound.Building == Building.MassProductionFactory || buildingFound.Building == Building.HardwareStore)
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
    }
}