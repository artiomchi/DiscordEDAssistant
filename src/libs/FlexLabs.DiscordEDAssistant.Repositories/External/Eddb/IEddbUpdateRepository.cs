using System;
using System.Collections.Generic;
using FlexLabs.DiscordEDAssistant.Models.External.Eddb;
using System.Threading.Tasks;

namespace FlexLabs.DiscordEDAssistant.Repositories.External.Eddb
{
    public interface IEddbUpdateRepository : IDisposable
    {
        void ClearAll();
        Task BulkUploadAsync(IEnumerable<Module> modules);
        Task BulkUploadAsync(IEnumerable<Models.External.Eddb.System> systems);
        void MergeAll();
        void MergeAllSystems();
    }
}
