using System;

namespace FlexLabs.DiscordEDAssistant.Models.External.Eddb
{
    public class Station
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int SystemID { get; set; }
        public LandingPadSize? MaxLandingPadSize { get; set; }
        public int? DistanceToStar { get; set; }
        public string Faction { get; set; }
        public string Government { get; set; }
        public string Allegiance { get; set; }
        public string State { get; set; }
        public int? TypeID { get; set; }
        public string TypeName { get; set; }
        public bool HasBlackmarket { get; set; }
        public bool HasMarket { get; set; }
        public bool HasRefuel { get; set; }
        public bool HasRepair { get; set; }
        public bool HasRearm { get; set; }
        public bool HasOutfitting { get; set; }
        public bool HasShipyard { get; set; }
        public bool HasDocking { get; set; }
        public bool HasCommodities { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? ShipyardUpdatedAt { get; set; }
        public DateTime? OutfittingUpdatedAt { get; set; }
        public DateTime? MarketUpdatedAt { get; set; }
        public bool IsPlanetary { get; set; }
        public string[] SellingShips { get; set; }
        public int[] SellingModules { get; set; }
        public string[] ImportCommodities { get; set; }
        public string[] ExportCommodities { get; set; }
        public string[] ProhibitedCommodities { get; set; }
        public string[] Economies { get; set; }
    }
}
