using FlexLabs.DiscordEDAssistant.Repositories.EFCore.Base;
using FlexLabs.DiscordEDAssistant.Repositories.External.Eddb;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FlexLabs.DiscordEDAssistant.Repositories.EFCompact.External.Eddb
{
    public class EFCoreEddbDataRepository : RepositoryBase, IEddbDataRepository
    {
        public EFCoreEddbDataRepository(EDAssistantDataContext context)
            : base(context)
        { }

        public IEnumerable<Models.External.Eddb.Station> FindClosestStationsWithModules(Models.External.Eddb.StarSystem system, IEnumerable<int> moduleIDs)
        {
            return DataContext.Set<Eddb_Stations_SearchClosestByModuleResult>()
                .FromSql("[eddb].[Modules_SearchClosestTo]", 
                    system.X, 
                    system.Y, 
                    system.Z, 
                    moduleIDs.Select(i => new { ID = i }).AsDataReader())
                .Select(s => new Models.External.Eddb.Station
                {
                    ID = s.ID,
                    Name = s.Name,
                    DistanceToStar = s.DistanceToStar,
                    SystemID = s.SystemID,
                    SystemName = s.SystemName,
                    DistanceToSystem = s.DistanceToSystem,
                });
        }

        public int? FindModuleID(string name)
        {
            return DataContext.Eddb_Modules
                .Where(m => m.Name == name)
                .Select(m => (int?)m.ID)
                .FirstOrDefault();
        }

        public Models.External.Eddb.StarSystem GetSystem(string name)
        {
            return DataContext.Eddb_StarSystems
                .Where(s => s.Name == name)
                .Select(s => new Models.External.Eddb.StarSystem
                {
                    ID = s.ID,
                    Name = s.Name,
                    X = s.X,
                    Y = s.Y,
                    Z = s.Z,
                    IsPopulated = s.IsPopulated,
                })
                .FirstOrDefault();
        }
    }
}
