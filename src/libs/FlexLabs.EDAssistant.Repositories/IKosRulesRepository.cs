using System;
using System.Threading.Tasks;

namespace FlexLabs.EDAssistant.Repositories
{
    public interface IKosRulesRepository : IDisposable
    {
        Task DeleteAsync(ulong serverID, string user);
        Task DeleteAsync(ulong serverID, ulong userID);
        Task<string> LoadAsync(ulong serverID, string user);
        Task<string> LoadAsync(ulong serverID, ulong userID);
        Task SetAsync(ulong serverID, string user, ulong authorID, string rule);
        Task SetAsync(ulong serverID, ulong userID, ulong authorID, string rule);
    }
}
