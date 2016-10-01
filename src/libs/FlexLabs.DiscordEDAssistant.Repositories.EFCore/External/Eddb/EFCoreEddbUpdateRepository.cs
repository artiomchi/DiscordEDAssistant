using FlexLabs.DiscordEDAssistant.Repositories.EFCore.Base;
using FlexLabs.DiscordEDAssistant.Repositories.External.Eddb;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Data;
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

        public Task BulkUploadAsync(IEnumerable<Models.External.Eddb.Module> modules)
        {
            var entities = modules
                .Select(m => new Upload_Eddb_Module
                {
                    ID = m.ID,
                    Name = m.Name,
                    Price = m.Price,
                    Class = m.Class,
                    Rating = m.Rating,
                    WeaponMode = (byte?)m.WeaponMode,
                    MissileType = m.MissileType,
                    Mass = m.Mass,
                    Power = m.Power,
                    Ship = m.Ship,
                    GroupID = m.GroupID,
                    GroupName = m.GroupName,
                    CategoryID = m.CategoryID,
                    CategoryName = m.CategoryName,
                });
            return BulkUploadEntitiesAsync(entities, "[upload].[Eddb_Modules]");
        }

        public Task BulkUploadAsync(IEnumerable<Models.External.Eddb.System> systems)
        {
            var entities = systems
                .Select(s => new Upload_Eddb_System
                {
                    ID = s.ID,
                    Name = s.Name,
                    X = s.X,
                    Y = s.Y,
                    Z = s.Z,
                    IsPopulated = s.IsPopulated,
                });
            return BulkUploadEntitiesAsync(entities, "[upload].[Eddb_Systems]");
        }

        public void MergeAll()
            => DataContext.Database.ExecuteSqlCommand("[upload].[Eddb_Merge]");

        public void MergeAllSystems()
        {
            SetLongTimeout();
            DataContext.Database.ExecuteSqlCommand("[upload].[Eddb_Merge_Systems]", 1);
        }
    }
}
