using FlexLabs.DiscordEDAssistant.Repositories.External.Eddb;
using System;

namespace FlexLabs.DiscordEDAssistant.Services.Integrations.Eddb
{
    public class EddbDataService : IDisposable
    {
        private readonly IEddbDataRepository _dataRepository;
        public EddbDataService(IEddbDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        public void Dispose() => _dataRepository.Dispose();

        public DiscordEDAssistant.Models.External.Eddb.System GetSystem(string name) => _dataRepository.GetSystem(name);
    }
}
