using FlexLabs.DiscordEDAssistant.Models;
using FlexLabs.DiscordEDAssistant.Repositories;
using System;

namespace FlexLabs.DiscordEDAssistant.Services.Data
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
