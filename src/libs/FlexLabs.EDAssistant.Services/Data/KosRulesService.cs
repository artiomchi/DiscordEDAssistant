using FlexLabs.EDAssistant.Repositories;
using System;
using System.Threading.Tasks;

namespace FlexLabs.EDAssistant.Services.Data
{
    public class KosRulesService : IDisposable
    {
        private readonly IKosRulesRepository _kosRulesRepository;
        public KosRulesService(IKosRulesRepository kosRulesRepository)
        {
            _kosRulesRepository = kosRulesRepository;
        }

        public void Dispose() => _kosRulesRepository.Dispose();

        public Task DeleteAsync(ulong serverID, string user) => _kosRulesRepository.DeleteAsync(serverID, user);
        public Task DeleteAsync(ulong serverID, ulong userID) => _kosRulesRepository.DeleteAsync(serverID, userID);
        public Task<string> LoadAsync(ulong serverID, string user) => _kosRulesRepository.LoadAsync(serverID, user);
        public Task<string> LoadAsync(ulong serverID, ulong userID) => _kosRulesRepository.LoadAsync(serverID, userID);
        public Task SetAsync(ulong serverID, string user, ulong authorID, string rule) => _kosRulesRepository.SetAsync(serverID, user, authorID, rule);
        public Task SetAsync(ulong serverID, ulong userID, ulong authorID, string rule) => _kosRulesRepository.SetAsync(serverID, userID, authorID, rule);
    }
}
