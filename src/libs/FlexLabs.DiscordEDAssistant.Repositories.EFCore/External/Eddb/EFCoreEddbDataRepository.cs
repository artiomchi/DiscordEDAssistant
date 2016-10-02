using FlexLabs.DiscordEDAssistant.Repositories.EFCore.Base;
using FlexLabs.DiscordEDAssistant.Repositories.External.Eddb;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System;

namespace FlexLabs.DiscordEDAssistant.Repositories.EFCompact.External.Eddb
{
    public class EFCoreEddbDataRepository : RepositoryBase, IEddbDataRepository
    {
        public EFCoreEddbDataRepository(EDAssistantDataContext context)
            : base(context)
        { }

        public async Task<IEnumerable<Models.External.Eddb.Station>> FindClosestStationsWithModulesAsync(Models.External.Eddb.StarSystem system, IEnumerable<int> moduleIDs)
        {
            using (var conn = new SqlConnection(EDAssistantDataContext.ConnectionString))
            using (var comm = new SqlCommand("[eddb].[Stations_SearchClosestByModule]", conn) { CommandType = System.Data.CommandType.StoredProcedure })
            {
                comm.Parameters.Add(new SqlParameter("@x", system.X));
                comm.Parameters.Add(new SqlParameter("@y", system.Y));
                comm.Parameters.Add(new SqlParameter("@z", system.Z));
                comm.Parameters.Add(new SqlParameter("@moduleIDs", moduleIDs.Select(i => new { ID = i }).ToDataTable()) { TypeName = "dbo.TVP_INT" });
                comm.Parameters.Add(new SqlParameter("@take", 15));

                await conn.OpenAsync();

                var result = new List<Models.External.Eddb.Station>();
                using (var reader = await comm.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new Models.External.Eddb.Station
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            DistanceToStar = !reader.IsDBNull(2) ? reader.GetInt32(2) : (int?)null,
                            MaxLandingPadSize = !reader.IsDBNull(3) ? (Models.External.Eddb.LandingPadSize)reader.GetByte(3) : (Models.External.Eddb.LandingPadSize?)null,
                            IsPlanetary = reader.GetBoolean(4),
                            MarketUpdatedAt = !reader.IsDBNull(5) ? reader.GetDateTime(5) : (DateTime?)null,
                            SystemID = reader.GetInt32(6),
                            SystemName = reader.GetString(7),
                            DistanceToSystem = Convert.ToSingle(reader.GetDouble(8)),
                        });
                    }
                }
                return result;
            }
        }

        public int? FindModuleID(string name)
        {
            return DataContext.Eddb_Modules
                .Where(m => m.FullName == name)
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
