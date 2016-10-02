using FlexLabs.DiscordEDAssistant.Repositories.EFCore.Base;
using FlexLabs.DiscordEDAssistant.Repositories.External.Eddb;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FlexLabs.DiscordEDAssistant.Repositories.EFCompact.External.Eddb
{
    public class EFCoreEddbUpdateRepository : RepositoryBase, IEddbUpdateRepository
    {
        public EFCoreEddbUpdateRepository(EDAssistantDataContext context)
            : base(context)
        { }

        public void ClearAll()
            => DataContext.Database.ExecuteSqlCommand("[upload].[Eddb_Truncate]");

        public Task BulkUploadAsync(IEnumerable<Models.External.Eddb.Commodity> modules)
        {
            var entities = modules
                .Select(c => new Upload_Eddb_Commodity
                {
                    ID = c.ID,
                    Name = c.Name,
                    AveragePrice = c.AveragePrice,
                    IsRare = c.IsRare,
                    CategoryID = c.CategoryID,
                    CategoryName = c.CategoryName,
                });
            return BulkUploadEntitiesAsync(entities, "[upload].[Eddb_Commodities]");
        }

        public Task BulkUploadAsync(IEnumerable<Models.External.Eddb.Module> modules)
        {
            var entities = modules
                .Select(m => new Upload_Eddb_Module
                {
                    ID = m.ID,
                    Name = m.Name,
                    Price = m.Price,
                    Class = m.Class,
                    Rating = (byte?)m.Rating,
                    WeaponMode = (byte?)m.WeaponMode,
                    MissileType = m.MissileType,
                    Mass = m.Mass,
                    Power = m.Power,
                    Ship = m.Ship,
                    GroupID = m.GroupID,
                    GroupName = m.GroupName,
                    CategoryID = m.CategoryID,
                    CategoryName = m.CategoryName,
                    FullName = m.Class.ToString() + m.Rating + " " + m.GroupName,
                });
            return BulkUploadEntitiesAsync(entities, "[upload].[Eddb_Modules]");
        }

        public Task BulkUploadAsync(IEnumerable<Models.External.Eddb.StarSystem> systems)
        {
            var entities = systems
                .Select(s => new Upload_Eddb_StarSystem
                {
                    ID = s.ID,
                    Name = s.Name,
                    X = s.X,
                    Y = s.Y,
                    Z = s.Z,
                    IsPopulated = s.IsPopulated,
                });
            return BulkUploadEntitiesAsync(entities, "[upload].[Eddb_StarSystems]");
        }

        public Task BulkUploadAsync(IEnumerable<Models.External.Eddb.Station> stations)
        {
            var entities = stations
                .Select(s => new Upload_Eddb_Station
                {
                    ID = s.ID,
                    Name = s.Name,
                    SystemID = s.SystemID,
                    MaxLandingPadSize = (byte?)s.MaxLandingPadSize,
                    DistanceToStar = s.DistanceToStar,
                    Faction = s.Faction,
                    Government = s.Government,
                    Allegiance = s.Allegiance,
                    State = s.State,
                    TypeID = s.TypeID,
                    TypeName = s.TypeName,
                    HasBlackmarket = s.HasBlackmarket,
                    HasMarket = s.HasMarket,
                    HasRefuel = s.HasRefuel,
                    HasRepair = s.HasRepair,
                    HasRearm = s.HasRearm,
                    HasOutfitting = s.HasOutfitting,
                    HasShipyard = s.HasShipyard,
                    HasDocking = s.HasDocking,
                    HasCommodities = s.HasCommodities,
                    UpdatedAt = s.UpdatedAt,
                    ShipyardUpdatedAt = s.ShipyardUpdatedAt,
                    OutfittingUpdatedAt = s.OutfittingUpdatedAt,
                    MarketUpdatedAt = s.MarketUpdatedAt,
                    IsPlanetary = s.IsPlanetary,
                });
            return BulkUploadEntitiesAsync(entities, "[upload].[Eddb_Stations]");
        }

        public Task BulkUploadStationModulesAsync(IEnumerable<Models.External.Eddb.Station> stations)
        {
            var entities = stations
                .SelectMany(s => s.SellingModules
                    .Select(sm => new Upload_Eddb_Stations_SellingModule
                    {
                        StationID = s.ID,
                        ModuleID = sm
                    }));
            return BulkUploadEntitiesAsync(entities, "[upload].[Eddb_Stations_SellingModules]");
        }

        public Task BulkUploadStationShipsAsync(IEnumerable<Models.External.Eddb.Station> stations)
        {
            var entities = stations
                .SelectMany(s => s.SellingShips
                    .Select(ss => new Upload_Eddb_Stations_SellingShip
                    {
                        StationID = s.ID,
                        Ship = ss
                    }));
            return BulkUploadEntitiesAsync(entities, "[upload].[Eddb_Stations_SellingShips]");
        }

        public void MergeAll()
        {
            SetLongTimeout();
            DataContext.Database.ExecuteSqlCommand("[upload].[Eddb_Merge]");
        }

        public void MergeAllSystems()
        {
            SetLongTimeout();
            DataContext.Database.ExecuteSqlCommand("[upload].[Eddb_Merge_StarSystems]", 1);
        }
    }
}
