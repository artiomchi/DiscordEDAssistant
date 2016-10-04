using FlexLabs.DiscordEDAssistant.Repositories.EFCore.Base;
using FlexLabs.DiscordEDAssistant.Repositories.External.Eddb;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;

namespace FlexLabs.DiscordEDAssistant.Repositories.EFCompact.External.Eddb
{
    public class EFCoreEddbUpdateRepository : RepositoryBase, IEddbUpdateRepository
    {
        public EFCoreEddbUpdateRepository(EDAssistantDataContext context)
            : base(context)
        { }

        public void ClearAll()
        {
            SetLongTimeout();
            DataContext.Database.ExecuteSqlCommand("[upload].[Eddb_Truncate]");
        }

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
            return BulkUploadEntitiesAsync("[upload].[Eddb_Commodities]", entities);
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
                    FullName = m.Class.ToString() + m.Rating + " " + (m.Name ?? m.GroupName),
                });
            return BulkUploadEntitiesAsync("[upload].[Eddb_Modules]", entities);
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
            return BulkUploadEntitiesAsync("[upload].[Eddb_StarSystems]", entities);
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
                    SellingModulesJson = s.SellingModulesJson,
                    SellingShipsJson = s.SellingShipsJson,
                });
            return BulkUploadEntitiesAsync("[upload].[Eddb_Stations]", entities);
        }

        public Task BulkUploadStationModulesAsync(IEnumerable<Tuple<int, int>> enumerable)
        {
            return BulkUploadEntitiesAsync("[upload].[Eddb_Stations_SellingModules]",
                enumerable.Select(t => new Upload_Eddb_Stations_SellingModule
                {
                    StationID = t.Item1,
                    ModuleID = t.Item2,
                }));
        }

        public Task BulkUploadStationShipsAsync(IEnumerable<Tuple<int, string>> enumerable)
        {
            return BulkUploadEntitiesAsync("[upload].[Eddb_Stations_SellingShips]",
                enumerable.Select(t => new Upload_Eddb_Stations_SellingShip
                {
                    StationID = t.Item1,
                    Ship = t.Item2,
                }));
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
