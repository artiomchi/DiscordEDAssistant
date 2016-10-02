using FlexLabs.DiscordEDAssistant.Repositories.EFCore.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FlexLabs.DiscordEDAssistant.Repositories.EFCore
{
    public class EFCoreKosRulesRepository : RepositoryBase, IKosRulesRepository
    {
        public EFCoreKosRulesRepository(EDAssistantDataContext context)
            : base(context)
        { }

        public Task DeleteAsync(ulong serverID, string user)
        {
            var dbRule = DataContext.KosSimpleRules
                .Where(s => s.ServerID == ConvertID(serverID))
                .Where(r => r.UserName == user)
                .SingleOrDefault();
            if (dbRule == null)
                return Task.FromResult(0);

            DataContext.KosSimpleRules.Remove(dbRule);
            return DataContext.SaveChangesAsync();
        }

        public Task DeleteAsync(ulong serverID, ulong userID)
        {
            var dbRule = DataContext.KosUserRules
                .Where(s => s.ServerID == ConvertID(serverID))
                .Where(r => r.UserID == ConvertID(userID))
                .SingleOrDefault();
            if (dbRule == null)
                return Task.FromResult(0);

            DataContext.KosUserRules.Remove(dbRule);
            return DataContext.SaveChangesAsync();
        }

        public Task<string> LoadAsync(ulong serverID, string user)
        {
            return DataContext.KosSimpleRules
                .Where(s => s.ServerID == ConvertID(serverID))
                .Where(r => r.UserName == user)
                .Select(r => r.Rule)
                .FirstOrDefaultAsync();
        }

        public Task<string> LoadAsync(ulong serverID, ulong userID)
        {
            return DataContext.KosUserRules
                .Where(s => s.ServerID == ConvertID(serverID))
                .Where(r => r.UserID == ConvertID(userID))
                .Select(r => r.Rule)
                .FirstOrDefaultAsync();
        }

        public Task SetAsync(ulong serverID, string user, ulong authorID, string rule)
        {
            var dbRule = DataContext.KosSimpleRules
                .Where(s => s.ServerID == ConvertID(serverID))
                .Where(r => r.UserName == user)
                .SingleOrDefault();
            if (dbRule == null)
                DataContext.KosSimpleRules.Add(dbRule = new KosSimpleRule { ServerID = ConvertID(serverID), UserName = user, AuthorID = ConvertID(authorID) });

            dbRule.Rule = rule;
            return DataContext.SaveChangesAsync();
        }

        public Task SetAsync(ulong serverID, ulong userID, ulong authorID, string rule)
        {
            var dbRule = DataContext.KosUserRules
                .Where(s => s.ServerID == ConvertID(serverID))
                .Where(r => r.UserID == ConvertID(userID))
                .SingleOrDefault();
            if (dbRule == null)
                DataContext.KosUserRules.Add(dbRule = new KosUserRule { ServerID = ConvertID(serverID), UserID = ConvertID(userID), AuthorID = ConvertID(authorID) });

            dbRule.Rule = rule;
            return DataContext.SaveChangesAsync();
        }
    }
}
