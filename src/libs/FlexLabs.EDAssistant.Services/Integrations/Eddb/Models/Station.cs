using FlexLabs.EDAssistant.Base.Extensions;
using Newtonsoft.Json;

namespace FlexLabs.EDAssistant.Services.Integrations.Eddb.Models
{
    public class Station
    {
        public int id { get; set; }
        public string name { get; set; }
        public int system_id { get; set; }
        public EDAssistant.Models.External.Eddb.LandingPadSize? max_landing_pad_size { get; set; }
        public int? distance_to_star { get; set; }
        public string faction { get; set; }
        public string government { get; set; }
        public string allegiance { get; set; }
        public string state { get; set; }
        public int? type_id { get; set; }
        public string type { get; set; }
        public bool has_blackmarket { get; set; }
        public bool has_market { get; set; }
        public bool has_refuel { get; set; }
        public bool has_repair { get; set; }
        public bool has_rearm { get; set; }
        public bool has_outfitting { get; set; }
        public bool has_shipyard { get; set; }
        public bool has_docking { get; set; }
        public bool has_commodities { get; set; }
        public int updated_at { get; set; }
        public int? shipyard_updated_at { get; set; }
        public int? outfitting_updated_at { get; set; }
        public int? market_updated_at { get; set; }
        public bool is_planetary { get; set; }
        public string[] selling_ships { get; set; }
        public int[] selling_modules { get; set; }
        public string[] import_commodities { get; set; }
        public string[] export_commodities { get; set; }
        public string[] prohibited_commodities { get; set; }
        public string[] economies { get; set; }

        public EDAssistant.Models.External.Eddb.Station Translate()
            => new EDAssistant.Models.External.Eddb.Station
            {
                ID = id,
                Name = name,
                SystemID = system_id,
                MaxLandingPadSize = max_landing_pad_size,
                DistanceToStar = distance_to_star,
                Faction = faction,
                Government = government,
                Allegiance = allegiance,
                State = state,
                TypeID = type_id,
                TypeName = type,
                HasBlackmarket = has_blackmarket,
                HasMarket = has_market,
                HasRefuel = has_refuel,
                HasRepair = has_repair,
                HasRearm = has_rearm,
                HasOutfitting = has_outfitting,
                HasShipyard = has_shipyard,
                HasDocking = has_docking,
                HasCommodities = has_commodities,
                UpdatedAt = updated_at.UnixTimeStampToDateTime(),
                ShipyardUpdatedAt = shipyard_updated_at?.UnixTimeStampToDateTime(),
                OutfittingUpdatedAt = outfitting_updated_at?.UnixTimeStampToDateTime(),
                MarketUpdatedAt = market_updated_at?.UnixTimeStampToDateTime(),
                IsPlanetary = is_planetary,
                SellingShips = selling_ships,
                SellingModules = selling_modules,
                ImportCommodities = import_commodities,
                ExportCommodities = export_commodities,
                ProhibitedCommodities = prohibited_commodities,
                Economies = economies,
                SellingModulesJson = JsonConvert.SerializeObject(selling_modules),
                SellingShipsJson = JsonConvert.SerializeObject(selling_ships),
            };
    }
}
