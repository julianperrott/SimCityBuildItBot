using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimCityBuildItBot.Bot
{
    public class Craftsman
    {
        private ILog log;

        private BuildingSelector buildingSelector;
        private NavigateToBuilding navigateToBuilding;
        private Touch touch;
        private CommerceResourceReader resourceReader;
        private List<CommerceItemBuild> buildItemList;

        public Craftsman(ILog log, BuildingSelector buildingSelector,
         NavigateToBuilding navigateToBuilding,
         Touch touch,
         CommerceResourceReader resourceReader,
         List<CommerceItemBuild> buildItemList)
        {
            this.log = log;
            this.buildingSelector = buildingSelector;
            this.navigateToBuilding = navigateToBuilding;
            this.touch = touch;
            this.resourceReader = resourceReader;
            this.buildItemList = buildItemList;
        }
        private List<Bot.FactoryResource> BuildItem(CommerceItem item)
        {
            return BuildItem(item, Bot.Location.ProductionQueue);
        }
        private List<Bot.FactoryResource> BuildItem(CommerceItem item, Bot.Location productionLocation)
        {
            CommerceItemBuild buildItem = buildItemList.Where(b => b.CommerceItem == item).FirstOrDefault();
            if (buildItem == null)
            {
                throw new ArgumentOutOfRangeException("CommerceItem item");
            }

            log.Info("Checking if I can Building item " + buildItem.Button.ToString() + " at " + buildItem.Building.ToString());

            var store = BuildingMatch.Create().Where(b => b.Building == buildItem.Building).FirstOrDefault();
            SelectBuilding(store);
            var required = resourceReader.GetRequiredResources(buildItem.Button, buildItem.Resources);
            log.Info("It requires : " + string.Join(",", required));
            if (required.Count > 0)
            {
                log.Info("I can't I need: " + string.Join(",", required));
                return required;
            }
            log.Info("I can ...");

            log.Info("Building item " + buildItem.Button.ToString());
            touch.Swipe(buildItem.Button, buildItem.Button, productionLocation, 4, true);

            var requiredFinal = resourceReader.GetRequiredResources(buildItem.Button, buildItem.Resources);
            log.Info("Finished build item, I need to build: " + string.Join(",", requiredFinal));
            return requiredFinal.ToList();
        }

        public void PickUpItems()
        {
            touch.ClickAt(Bot.Location.CentreMap, 100);

            for (int i = 0; i < 10; i++)
            {
                if (buildingSelector.GetBuilding() != null)
                {
                    break;
                }
                touch.ClickAt(Bot.Location.CentreMap, 100);
            }
        }

        private void SelectBuilding(BuildingMatch buildingMatch)
        {
            navigateToBuilding.NavigateTo(buildingMatch, 1);

            // pick up any items
            PickUpItems();

            // reselect
            navigateToBuilding.NavigateTo(buildingMatch, 1);
        }

        private void BuildFactoryItem(FactoryResource resource)
        {
            BuildFactoryItemIfNeeded(resource, new List<FactoryResource> { resource });
        }

        private Bot.Location GetResourceLocation(FactoryResource resource)
        {
            switch (resource)
            {
                case FactoryResource.Metal: return Bot.Location.ButtonLeftInner1;
                case FactoryResource.Wood: return Bot.Location.ButtonLeftInner2;
                case FactoryResource.Plastic: return Bot.Location.ButtonLeftInner3;

                case FactoryResource.Textiles: return Bot.Location.ButtonLeftOuter1;
                case FactoryResource.SugarAndSpices: return Bot.Location.ButtonLeftOuter2;
                case FactoryResource.Glass: return Bot.Location.ButtonLeftOuter3;

                case FactoryResource.Seeds: return Bot.Location.ButtonRightInner1;
                case FactoryResource.Minerals: return Bot.Location.ButtonRightInner2;
                case FactoryResource.Chemicals: return Bot.Location.ButtonRightInner3;
            }

            throw new Exception("Unknown factory resource location for " + resource.ToString());
        }

        private void BuildFactoryItemIfNeeded(FactoryResource resource, IEnumerable<FactoryResource> factoryList)
        {
            var alwaysBuildItem = !new List<FactoryResource> { FactoryResource.Plastic, FactoryResource.Metal, FactoryResource.Wood, FactoryResource.Seeds }
                .Contains(resource);

            if (factoryList.Contains(resource))
            {
                log.Info("Picking up items");
                PickUpItems();
            }
            else
            {
                log.Info("Don't need to build " + resource.ToString());
            }

            if (alwaysBuildItem || factoryList.Contains(resource))
            {
                BuildingMatch buildingMatch = buildingSelector.SelectABuilding(", Trying to build " + resource.ToString());
                if (buildingMatch != null && buildingMatch.BuildingType == BuildingType.Factory)
                {
                    var button = GetResourceLocation(resource);
                    touch.Swipe(button, Bot.Location.FactoryQueuePositionStart, Bot.Location.FactoryQueuePositionend, 10, false);
                }
            }
        }

        internal bool CraftLevel12()
        {
            var startFactory = BuildingMatch.Create().Where(b => b.Building == Building.BasicFactory).FirstOrDefault();

            SelectBuilding(startFactory);
            for (var i = 0; i < 8; i++)
            {
                PickupItemsFromNextFactory();
            }
            SelectBuilding(startFactory);

            // Harware store
            var items1 = BuildItem(CommerceItem.Hammer, Bot.Location.ProductionQueueL8);


            // Building Supplies Store
            BuildItem(CommerceItem.Nails, Bot.Location.ProductionQueueL8);
            BuildItem(CommerceItem.Nails, Bot.Location.ProductionQueueL8);
            var items3 = BuildItem(CommerceItem.Nails, Bot.Location.ProductionQueueL8);

            //Furniture store
            var items4=  BuildItem(CommerceItem.Chair, Bot.Location.ProductionQueueL8);

            // Farmers market
            var items2 = BuildItem(CommerceItem.Vegetable, Bot.Location.ProductionQueueL8);

            var requiredResources = items1.Concat(items2)
                .Concat(items3)
                .Concat(items4)
                .Distinct()
                .Where(r => r != FactoryResource.Ignore);

            log.Info("The factory needs to build: " + string.Join(",", requiredResources));

            if (requiredResources.Count() > 0)
            {
                SelectBuilding(startFactory);

                if (requiredResources.Contains(FactoryResource.Metal))
                {
                    BuildAtAllL12Factories(FactoryResource.Metal);
                }
                else if (requiredResources.Contains(FactoryResource.Wood))
                {
                    BuildAtAllL12Factories(FactoryResource.Wood);
                }
                else
                {
                    BuildFactoryItem(FactoryResource.Seeds);
                    touch.ClickAt(Bot.Location.RightButton);
                    BuildFactoryItem(FactoryResource.Seeds);
                    touch.ClickAt(Bot.Location.RightButton);
                    BuildFactoryItem(FactoryResource.Seeds);
                    touch.ClickAt(Bot.Location.RightButton);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Craft()
        {
            var massProductionFactory = BuildingMatch.Create().Where(b => b.Building == Building.MassProductionFactory).FirstOrDefault();

            SelectBuilding(massProductionFactory);
            for (var i = 0; i < 8; i++)
            {
                PickupItemsFromNextFactory();
            }

            // Harware store
            var items1 = BuildItem(CommerceItem.Hammer);

            //Furniture store
            var items2 = BuildItem(CommerceItem.Table);
            if (items2.Contains(FactoryResource.Ignore))
            {
                items2 = BuildItem(CommerceItem.Chair);
            }

            // Donut shop
            //var items6 = BuildItem(CommerceItem.BreadRoll);
            //items6 = BuildItem(CommerceItem.GreenSmoothie);
            var items6 = BuildItem(CommerceItem.Donut);

            // Home appliances
            var items8 = BuildItem(CommerceItem.BBQGrill);

            // Farmers market
            var items4 = BuildItem(CommerceItem.FlourBag);

            // Gardening supplies
            var items7 = BuildItem(CommerceItem.TreeSapling);
            if (items7.Contains(FactoryResource.Ignore))
            {
                items7 = BuildItem(CommerceItem.GardenFurniture);
            }

            // Fashion store
            var items5 = BuildItem(CommerceItem.Cap);
            if (items5.Contains(FactoryResource.Ignore))
            {
                items5 = BuildItem(CommerceItem.Shoe);
            }
            if (items5.Contains(FactoryResource.Ignore))
            {
                items5 = BuildItem(CommerceItem.Watch);
            }

            // Fast food restaurant
            BuildItem(CommerceItem.IceCreamSandwich);

            // Building Supplies Store
            BuildItem(CommerceItem.Nails);
            BuildItem(CommerceItem.Nails);
            var items3 = BuildItem(CommerceItem.Nails);

            var requiredResources = items1.Concat(items2)
                .Concat(items3).Concat(items4).Concat(items5).Concat(items6).Concat(items7).Concat(items8)
                .Distinct()
                .Where(r => r != FactoryResource.Ignore);

            log.Info("The factory needs to build: " + string.Join(",", requiredResources));

            if (requiredResources.Count() > 0)
            {
                SelectBuilding(massProductionFactory);

                if (requiredResources.Contains(FactoryResource.Metal))
                {
                    BuildAtAllFactories(FactoryResource.Metal);
                }
                else if (requiredResources.Contains(FactoryResource.Wood))
                {
                    BuildAtAllFactories(FactoryResource.Wood);
                }
                else
                {
                    BuildFactoryItemIfNeeded(FactoryResource.Metal, requiredResources);
                    touch.ClickAt(Bot.Location.RightButton);
                    BuildFactoryItemIfNeeded(FactoryResource.Wood, requiredResources);
                    touch.ClickAt(Bot.Location.RightButton);

                    var slowItem = FactoryResource.Textiles;
                    BuildFactoryItem(slowItem);
                    touch.ClickAt(Bot.Location.RightButton);
                    BuildFactoryItem(slowItem);
                    touch.ClickAt(Bot.Location.RightButton);
                    BuildFactoryItem(slowItem);
                    touch.ClickAt(Bot.Location.RightButton);

                    BuildFactoryItem(FactoryResource.SugarAndSpices);
                    touch.ClickAt(Bot.Location.RightButton);
                    BuildFactoryItemIfNeeded(FactoryResource.Seeds, requiredResources);
                    touch.ClickAt(Bot.Location.RightButton);
                    BuildFactoryItemIfNeeded(FactoryResource.Metal, requiredResources);
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        private void BuildAtAllFactories(FactoryResource resource)
        {
            BuildFactoryItem(resource);
            touch.ClickAt(Bot.Location.RightButton);
            BuildFactoryItem(resource);
            touch.ClickAt(Bot.Location.RightButton);


            touch.ClickAt(Bot.Location.RightButton);
            touch.ClickAt(Bot.Location.RightButton);
            touch.ClickAt(Bot.Location.RightButton);
            touch.ClickAt(Bot.Location.RightButton);

            BuildFactoryItem(resource);
            touch.ClickAt(Bot.Location.RightButton);
            BuildFactoryItem(resource); ;
        }

        private void PickupItemsFromNextFactory()
        {
            touch.ClickAt(Bot.Location.RightButton);
            PickUpItems();
            buildingSelector.SelectABuilding(" picking up factory Items");
        }

        public bool CraftLevel8()
        {
            var startFactory = BuildingMatch.Create().Where(b => b.Building == Building.SmallFactory).FirstOrDefault();

            SelectBuilding(startFactory);
            for (var i = 0; i < 8; i++)
            {
                PickupItemsFromNextFactory();
            }
            SelectBuilding(startFactory);

            // Harware store
            var items1 = BuildItem(CommerceItem.Hammer, Bot.Location.ProductionQueueL8);

            // Farmers market
            var items2 = BuildItem(CommerceItem.Vegetable, Bot.Location.ProductionQueueL8);

            // Building Supplies Store
            BuildItem(CommerceItem.Nails, Bot.Location.ProductionQueueL8);
            BuildItem(CommerceItem.Nails, Bot.Location.ProductionQueueL8);
            var items3 = BuildItem(CommerceItem.Nails, Bot.Location.ProductionQueueL8);

            var requiredResources = items1.Concat(items2)
                .Concat(items3)
                .Distinct()
                .Where(r => r != FactoryResource.Ignore);

            log.Info("The factory needs to build: " + string.Join(",", requiredResources));

            if (requiredResources.Count() > 0)
            {
                SelectBuilding(startFactory);

                if (requiredResources.Contains(FactoryResource.Metal))
                {
                    BuildAtAllL8Factories(FactoryResource.Metal);
                }
                else if (requiredResources.Contains(FactoryResource.Wood))
                {
                    BuildAtAllL8Factories(FactoryResource.Wood);
                }
                else
                {
                    BuildFactoryItem(FactoryResource.Seeds);
                    touch.ClickAt(Bot.Location.RightButton);
                    BuildFactoryItem(FactoryResource.Seeds);
                    touch.ClickAt(Bot.Location.RightButton);
                    BuildFactoryItem(FactoryResource.Seeds);
                    touch.ClickAt(Bot.Location.RightButton);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private void BuildAtAllL8Factories(FactoryResource resource)
        {
            BuildFactoryItem(resource);
            touch.ClickAt(Bot.Location.RightButton);
            BuildFactoryItem(resource);
            touch.ClickAt(Bot.Location.RightButton);
            BuildFactoryItem(resource);
            touch.ClickAt(Bot.Location.RightButton);
            BuildFactoryItem(resource);
            touch.ClickAt(Bot.Location.RightButton);
            BuildFactoryItem(resource);
            touch.ClickAt(Bot.Location.RightButton);
            BuildFactoryItem(resource);
        }

        private void BuildAtAllL12Factories(FactoryResource resource)
        {
            BuildFactoryItem(resource);
            touch.ClickAt(Bot.Location.RightButton);
            BuildFactoryItem(resource);
            touch.ClickAt(Bot.Location.RightButton);
            BuildFactoryItem(resource);
            touch.ClickAt(Bot.Location.RightButton);
            BuildFactoryItem(resource);
            touch.ClickAt(Bot.Location.RightButton);
            BuildFactoryItem(resource);
            touch.ClickAt(Bot.Location.RightButton);
            BuildFactoryItem(resource);
            touch.ClickAt(Bot.Location.RightButton);
            BuildFactoryItem(resource);
        }
    }
}