using FlexLabs.EDAssistant.Repositories.EFCore.Base;
using System;
using System.Linq;

namespace FlexLabs.EDAssistant.Repositories.EFCore
{
    public class EFCoreServersRepository : RepositoryBase, IServersRepository
    {
        public EFCoreServersRepository(EDAssistantDataContext context)
            : base(context)
        { }

        public Models.Data.Server Load(ulong serverID)
        {
            return DataContext.Servers
                .Where(s => s.ID == ConvertID(serverID))
                .Select(s => new Models.Data.Server
                {
                    ID = ConvertID(s.ID),
                    CommandPrefix = s.CommandPrefix,
                    WelcomeMessage = s.WelcomeMessage,
                })
                .FirstOrDefault();
        }

        public void Update(ulong serverID, Action<Models.Data.Server> updater)
        {
            var dbServer = DataContext.Servers.SingleOrDefault(s => s.ID == ConvertID(serverID));
            if (dbServer == null)
                DataContext.Servers.Add(dbServer = new Server { ID = ConvertID(serverID) });

            var server = new Models.Data.Server
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
