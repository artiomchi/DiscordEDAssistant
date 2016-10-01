using System;

namespace FlexLabs.DiscordEDAssistant.Repositories.External.Eddb
{
    public interface IEddbDataRepository : IDisposable
    {
        Models.External.Eddb.StarSystem GetSystem(string name);
    }
}
