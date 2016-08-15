namespace SimCityBuildItBot.Bot
{
    using System.Collections.Generic;

    public class BuildingMatch
    {
        public Building Building { get; set; }
        public BuildingType BuildingType { get; set; }
        public List<string> Fragments { get; set; }

        public static List<BuildingMatch> Create()
        {
            var buildingMatches = new List<BuildingMatch>();

            buildingMatches.Add(new BuildingMatch { Building = Building.GardeningSupplies, BuildingType = BuildingType.Commercial, Fragments = new List<string> { "gard", "ening", "arden" } });
            buildingMatches.Add(new BuildingMatch { Building = Building.FurnitureStore, BuildingType = BuildingType.Commercial, Fragments = new List<string> { "furn", "rnit", "ture" } });
            buildingMatches.Add(new BuildingMatch { Building = Building.BuildingSuppliesStore, BuildingType = BuildingType.Commercial, Fragments = new List<string> { "build", "ding", "ildi" } });
            buildingMatches.Add(new BuildingMatch { Building = Building.FarmersMarket, BuildingType = BuildingType.Commercial, Fragments = new List<string> { "farm", "rmers", "mark", "rket" } });
            buildingMatches.Add(new BuildingMatch { Building = Building.HardwareStore, BuildingType = BuildingType.Commercial, Fragments = new List<string> { "hard", "ware", "rdwa" } });
            buildingMatches.Add(new BuildingMatch { Building = Building.DonutShop, BuildingType = BuildingType.Commercial, Fragments = new List<string> { "don", "nut", "onu" } });
            buildingMatches.Add(new BuildingMatch { Building = Building.FashionStore, BuildingType = BuildingType.Commercial, Fragments = new List<string> { "fash", "hion", "ashi", "ion stor" } });
            buildingMatches.Add(new BuildingMatch { Building = Building.FastFoodRestaurant, BuildingType = BuildingType.Commercial, Fragments = new List<string> { "Fast", "Food", "Rest", "aurant" } });

            buildingMatches.Add(new BuildingMatch { Building = Building.BasicFactory, BuildingType = BuildingType.Factory, Fragments = new List<string> { "bas", "sic", "ic fact" } });
            buildingMatches.Add(new BuildingMatch { Building = Building.MassProductionFactory, BuildingType = BuildingType.Factory, Fragments = new List<string> { "mass", "prod", "ion fact" } });
            buildingMatches.Add(new BuildingMatch { Building = Building.HighTechFactory, BuildingType = BuildingType.Factory, Fragments = new List<string> { "high", "tech", "ech fact" } });
            return buildingMatches;
        }
    }
}