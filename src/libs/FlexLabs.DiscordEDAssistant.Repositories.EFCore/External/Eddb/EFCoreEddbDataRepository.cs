using FlexLabs.DiscordEDAssistant.Repositories.EFCore.Base;
using FlexLabs.DiscordEDAssistant.Repositories.External.Eddb;
using System.Linq;

namespace FlexLabs.DiscordEDAssistant.Repositories.EFCompact.External.Eddb
{
    public class EFCoreEddbDataRepository : RepositoryBase, IEddbDataRepository
    {
        public EFCoreEddbDataRepository(EDAssistantDataContext context)
            : base(context)
        { }

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
