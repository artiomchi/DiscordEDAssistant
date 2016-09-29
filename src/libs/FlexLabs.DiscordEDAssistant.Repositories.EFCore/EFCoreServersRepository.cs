using FlexLabs.DiscordEDAssistant.Repositories.EFCore.Base;
using System;
using System.Linq;

namespace FlexLabs.DiscordEDAssistant.Repositories.EFCore
{
    public class EFCoreServersRepository : RepositoryBase, IServersRepository
    {
        public EFCoreServersRepository(EDAssistantDataContext context)
            : base(context)
        { }

        public Models.Server Load(ulong serverID)
        {
            return DataContext.Servers
                .Where(s => s.ID == ConvertID(serverID))
                .Select(s => new Models.Server
                {
                    ID = ConvertID(s.ID),
                    CommandPrefix = s.CommandPrefix,
                    WelcomeMessage = s.WelcomeMessage,
                })
                .FirstOrDefault();
        }

        public void Update(ulong serverID, Action<Models.Server> updater)
        {
            var dbServer = DataContext.Servers.SingleOrDefault(s => s.ID == ConvertID(serverID));
            if (dbServer == null)
                DataContext.Servers.Add(dbServer = new Server { ID = ConvertID(serverID) });

            var server = new Models.Server
            {
                CommandPrefix = dbServer.CommandPrefix,
                WelcomeMessage = dbServer.WelcomeMessage,
            };
            updater(server);

            dbServer.CommandPrefix = server.CommandPrefix;
            dbServer.WelcomeMessage = server.WelcomeMessage;
            DataContext.SaveChanges();
        }
    }
}
