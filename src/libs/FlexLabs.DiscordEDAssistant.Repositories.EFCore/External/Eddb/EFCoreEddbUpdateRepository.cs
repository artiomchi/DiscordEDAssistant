using FlexLabs.DiscordEDAssistant.Repositories.EFCore.Base;
using FlexLabs.DiscordEDAssistant.Repositories.External.Eddb;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace FlexLabs.DiscordEDAssistant.Repositories.EFCompact.External.Eddb
{
    public class EFCoreEddbUpdateRepository : RepositoryBase, IEddbUpdateRepository
    {
        public EFCoreEddbUpdateRepository(EDAssistantDataContext context)
            : base(context)
        { }

        public void ClearAll()
            => DataContext.Database.ExecuteSqlCommand("[upload].[Eddb_Truncate]");

        public void UploadAll(IEnumerable<Models.External.Eddb.Module> modules)
        {
            DataContext.Upload_Eddb_Modules.AddRange(modules
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
                }));
            DataContext.SaveChanges();
        }

        public void UploadAll(IEnumerable<Models.External.Eddb.System> systems)
        {
            DataContext.Upload_Eddb_Systems.AddRange(systems
                .Select(s => new Upload_Eddb_System
                {
                    ID = s.ID,
                    Name = s.Name,
                    X = s.X,
                    Y = s.Y,
                    Z = s.Z,
                    IsPopulated = s.IsPopulated,
                }));
            DataContext.SaveChanges();
        }

        public void MergeAll()
            => DataContext.Database.ExecuteSqlCommand("[upload].[Eddb_Merge]");

        public void MergeAllSystems()
            => DataContext.Database.ExecuteSqlCommand("[upload].[Eddb_Merge_Systems]", 1);
    }
}
