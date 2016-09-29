using FlexLabs.DiscordEDAssistant.Repositories.EFCompact.Base;
using System;
using System.Linq;

namespace FlexLabs.DiscordEDAssistant.Repositories.EFCompact
{
    public class EFCompactServersRepository : RepositoryBase, IServersRepository
    {
        public EFCompactServersRepository(EDAssistantDataContext context)
            : base(context)
        { }

        public Models.Server Load(ulong serverID)
        {
            return DataContext.Servers
                .Where(s => s.ServerID == ConvertID(serverID))
                .Select(s => new Models.Server
                {
                    ID = ConvertID(s.ServerID),
                    CommandPrefix = s.CommandPrefix,
                })
                .FirstOrDefault();
        }

        public void Update(ulong serverID, Action<Models.Server> updater)
        {
            var dbServer = DataContext.Servers.SingleOrDefault(s => s.ServerID == ConvertID(serverID));
            if (dbServer == null)
                DataContext.Servers.Add(dbServer = new Server { ServerID = ConvertID(serverID) });

            var server = new Models.Server
            {
                CommandPrefix = dbServer.CommandPrefix,
            };
            updater(server);

            dbServer.CommandPrefix = server.CommandPrefix;
            DataContext.SaveChanges();
        }
    }
}
