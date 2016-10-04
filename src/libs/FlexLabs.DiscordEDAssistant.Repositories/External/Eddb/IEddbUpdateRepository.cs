using System;
using System.Collections.Generic;
using FlexLabs.DiscordEDAssistant.Models.External.Eddb;
using System.Threading.Tasks;

namespace FlexLabs.DiscordEDAssistant.Repositories.External.Eddb
{
    public interface IEddbUpdateRepository : IDisposable
    {
        void ClearAll();
        Task BulkUploadAsync(IEnumerable<Commodity> modules);
        Task BulkUploadAsync(IEnumerable<Module> modules);
        Task BulkUploadAsync(IEnumerable<StarSystem> systems);
        Task BulkUploadAsync(IEnumerable<Station> systems);
        Task BulkUploadStationModulesAsync(IEnumerable<Tuple<int, int>> enumerable);
        Task BulkUploadStationShipsAsync(IEnumerable<Tuple<int, string>> enumerable);
        void MergeAll();
        void MergeAllSystems();
    }
}
