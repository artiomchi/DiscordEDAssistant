using FlexLabs.EDAssistant.Models.Data;
using System;

namespace FlexLabs.EDAssistant.Repositories
{
    public interface IServersRepository : IDisposable
    {
        Server Load(ulong serverID);
        void Update(ulong serverID, Action<Server> updater);
    }
}
