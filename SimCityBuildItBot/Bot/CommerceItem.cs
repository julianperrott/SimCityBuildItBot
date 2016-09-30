namespace SimCityBuildItBot.Bot
{
    using System.Collections.Generic;

    public enum CommerceItem
    {
        Hammer,
        Cap,
        Shoe,
        Watch,
        Donut,
        Chair,
        Nails,
        TreeSapling,
        Grass,
        GardenFurniture,
        FlourBag,
        Vegetable,
        Ladder,
        MeasuringTape,
        IceCreamSandwich,
        GreenSmoothie,
        Table,
        BBQGrill,
        BreadRoll
    }

    public class CommerceItemBuild
    {
        public CommerceItem CommerceItem { get; private set; }
        public Building Building { get; private set; }
        public Bot.Location Button { get; private set; }
        public List<Bot.FactoryResource> Resources { get; private set; }

        public CommerceItemBuild(CommerceItem item, Building building, Bot.Location button, List<Bot.FactoryResource> resources)
        {
            this.CommerceItem = item;
            this.Building = building;
            this.Button = button;
            this.Resources = resources;
        }

        public static List<CommerceItemBuild> CreateResourceList()
        {
            return new List<CommerceItemBuild>
            {
                new CommerceItemBuild(CommerceItem.Hammer ,Building.HardwareStore, Bot.Location.ButtonLeftInner1,  new List<Bot.FactoryResource>()
                {
                    Bot.FactoryResource.Metal,
                    Bot.FactoryResource.Wood
                }),
                 new CommerceItemBuild(CommerceItem.MeasuringTape ,Building.HardwareStore, Bot.Location.ButtonLeftInner2,  new List<Bot.FactoryResource>()
                {
                    Bot.FactoryResource.Metal,
                    Bot.FactoryResource.Plastic
                }),
                 new CommerceItemBuild(CommerceItem.Ladder ,Building.HardwareStore, Bot.Location.ButtonRightInner2,  new List<Bot.FactoryResource>()
                {
                    Bot.FactoryResource.Ignore,
                    Bot.FactoryResource.Metal
                }),
                new CommerceItemBuild(CommerceItem.Cap,Building.FashionStore, Bot.Location.ButtonLeftInner1, new List<Bot.FactoryResource>()
                {
                    Bot.FactoryResource.Textiles,
                    Bot.FactoryResource.Ignore
                }),
                new CommerceItemBuild(CommerceItem.Shoe,Building.FashionStore, Bot.Location.ButtonLeftInner3, new List<Bot.FactoryResource>()
                {
                    Bot.FactoryResource.Textiles,
                    Bot.FactoryResource.Plastic,
                    Bot.FactoryResource.Ignore
                }),
                new CommerceItemBuild(CommerceItem.Watch,Building.FashionStore, Bot.Location.ButtonLeftInner3, new List<Bot.FactoryResource>()
                {
                    Bot.FactoryResource.Plastic,
                    Bot.FactoryResource.Glass,
                    Bot.FactoryResource.Chemicals
                }),
                new CommerceItemBuild(CommerceItem.Donut,Building.DonutShop, Bot.Location.ButtonLeftInner1, new List<Bot.FactoryResource>()
                {
                    Bot.FactoryResource.Ignore,
                    Bot.FactoryResource.SugarAndSpices
                }),
                new CommerceItemBuild(CommerceItem.GreenSmoothie,Building.DonutShop, Bot.Location.ButtonLeftInner2, new List<Bot.FactoryResource>()
                {
                    Bot.FactoryResource.Ignore,
                    Bot.FactoryResource.Ignore
                }),
                new CommerceItemBuild(CommerceItem.BreadRoll,Building.DonutShop, Bot.Location.ButtonLeftInner3, new List<Bot.FactoryResource>()
                {
                    Bot.FactoryResource.Ignore,
                    Bot.FactoryResource.Ignore
                }),
                new CommerceItemBuild(CommerceItem.Chair,Building.FurnitureStore, Bot.Location.ButtonLeftInner1,  new List<Bot.FactoryResource>()
                {
                    Bot.FactoryResource.Wood,
                    Bot.FactoryResource.Metal,
                    Bot.FactoryResource.Ignore
                }),
                new CommerceItemBuild(CommerceItem.Table,Building.FurnitureStore, Bot.Location.ButtonLeftInner2,  new List<Bot.FactoryResource>()
                {
                    Bot.FactoryResource.Ignore,
                    Bot.FactoryResource.Ignore,
                    Bot.FactoryResource.Ignore
                }),
                new CommerceItemBuild(CommerceItem.Nails,Building.BuildingSuppliesStore, Bot.Location.ButtonLeftInner1,  new List<Bot.FactoryResource>()
                {
                    Bot.FactoryResource.Metal
                }),
                new CommerceItemBuild(CommerceItem.Grass,Building.GardeningSupplies, Bot.Location.ButtonLeftInner1,  new List<Bot.FactoryResource>()
                {
                    Bot.FactoryResource.Seeds,
                    Bot.FactoryResource.Ignore,
                }),
                new CommerceItemBuild(CommerceItem.TreeSapling,Building.GardeningSupplies, Bot.Location.ButtonLeftInner2,  new List<Bot.FactoryResource>()
                {
                    Bot.FactoryResource.Seeds,
                    Bot.FactoryResource.Ignore,
                }),
                new CommerceItemBuild(CommerceItem.GardenFurniture,Building.GardeningSupplies, Bot.Location.ButtonLeftInner3, new List<Bot.FactoryResource>()
                {
                    Bot.FactoryResource.Ignore,
                    Bot.FactoryResource.Plastic,
                    Bot.FactoryResource.Textiles
                }),
                new CommerceItemBuild(CommerceItem.FlourBag,Building.FarmersMarket, Bot.Location.ButtonLeftInner2, new List<Bot.FactoryResource>()
                {
                    Bot.FactoryResource.Seeds,
                    Bot.FactoryResource.Textiles
                }),
                new CommerceItemBuild(CommerceItem.Vegetable,Building.FarmersMarket, Bot.Location.ButtonLeftInner1, new List<Bot.FactoryResource>()
                {
                    Bot.FactoryResource.Seeds
                }),
                new CommerceItemBuild(CommerceItem.IceCreamSandwich,Building.FastFoodRestaurant, Bot.Location.ButtonLeftInner1, new List<Bot.FactoryResource>()
                {
                    Bot.FactoryResource.Ignore,
                    Bot.FactoryResource.Ignore
                }),
                 new CommerceItemBuild(CommerceItem.BBQGrill,Building.HomeAppliances, Bot.Location.ButtonLeftInner1, new List<Bot.FactoryResource>()
                {
                    Bot.FactoryResource.Metal,
                    Bot.FactoryResource.Ignore
                }),
            };
        }
    }
}