using FlexLabs.EDAssistant.Models.Data;
using FlexLabs.EDAssistant.Repositories;
using System;

namespace FlexLabs.EDAssistant.Services.Data
{
    public class ServersService : IDisposable
    {
        private readonly IServersRepository _serversRepository;
        public ServersService(IServersRepository serversRepository)
        {
            _serversRepository = serversRepository;
        }

        public void Dispose() => _serversRepository.Dispose();

        public Server Load(ulong serverID) => _serversRepository.Load(serverID);
        public void SetCommandPrefix(ulong serverID, string commandPrefix) => _serversRepository.Update(serverID, s => s.CommandPrefix = commandPrefix);
        public void SetWelcomeMessage(ulong serverID, string welcomeMessage) => _serversRepository.Update(serverID, s => s.WelcomeMessage = welcomeMessage);
    }
}
