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

        public async Task BulkUploadSystemsAsync(IEnumerable<Models.External.Eddb.System> systems)
        {
            using (SqlBulkCopy copy = new SqlBulkCopy(DataContext.Database.GetDbConnection().ConnectionString)
            {
                BulkCopyTimeout = 300,
                DestinationTableName = "[upload].[Eddb_Systems]",
                BatchSize = 3000,
            })
            using (var table = new DataTable())
            {
                copy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(nameof(Upload_Eddb_System.ID), nameof(Upload_Eddb_System.ID)));
                copy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(nameof(Upload_Eddb_System.Name), nameof(Upload_Eddb_System.Name)));
                copy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(nameof(Upload_Eddb_System.X), nameof(Upload_Eddb_System.X)));
                copy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(nameof(Upload_Eddb_System.Y), nameof(Upload_Eddb_System.Y)));
                copy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(nameof(Upload_Eddb_System.Z), nameof(Upload_Eddb_System.Z)));
                copy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(nameof(Upload_Eddb_System.IsPopulated), nameof(Upload_Eddb_System.IsPopulated)));

                table.Columns.Add(nameof(Upload_Eddb_System.ID));
                table.Columns.Add(nameof(Upload_Eddb_System.Name));
                table.Columns.Add(nameof(Upload_Eddb_System.X));
                table.Columns.Add(nameof(Upload_Eddb_System.Y));
                table.Columns.Add(nameof(Upload_Eddb_System.Z));
                table.Columns.Add(nameof(Upload_Eddb_System.IsPopulated));
                foreach (var system in systems)
                    table.Rows.Add(system.ID, system.Name, system.X, system.Y, system.Z, system.IsPopulated);

                await copy.WriteToServerAsync(table);
            }
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
