using FlexLabs.EDAssistant.Models.External.Eddb;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlexLabs.EDAssistant.Repositories.External.Eddb
{
    public interface IEddbDataRepository : IDisposable
    {
        StarSystem GetSystem(string name);
        Task<IEnumerable<Station>> FindClosestStationsWithModulesAsync(StarSystem system, IEnumerable<int> moduleIDs);
        int? FindModuleID(string name);
    }
}
