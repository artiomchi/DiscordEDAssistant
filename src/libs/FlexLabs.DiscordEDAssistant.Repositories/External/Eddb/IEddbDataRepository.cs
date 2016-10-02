using FlexLabs.DiscordEDAssistant.Models.External.Eddb;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlexLabs.DiscordEDAssistant.Repositories.External.Eddb
{
    public interface IEddbDataRepository : IDisposable
    {
        StarSystem GetSystem(string name);
        Task<IEnumerable<Station>> FindClosestStationsWithModulesAsync(StarSystem system, IEnumerable<int> moduleIDs);
        int? FindModuleID(string name);
    }
}
