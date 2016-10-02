using FlexLabs.DiscordEDAssistant.Models.External.Eddb;
using System;
using System.Collections.Generic;

namespace FlexLabs.DiscordEDAssistant.Repositories.External.Eddb
{
    public interface IEddbDataRepository : IDisposable
    {
        StarSystem GetSystem(string name);
        IEnumerable<Station> FindClosestStationsWithModules(StarSystem system, IEnumerable<int> moduleIDs);
        int? FindModuleID(string name);
    }
}
