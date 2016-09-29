using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace FlexLabs.DiscordEDAssistant.Repositories.EFCompact.Base
{
#if DEBUG
    public class EDAssistantDataContextFactory : IDbContextFactory<EDAssistantDataContext>
    {
        public EDAssistantDataContext Create(DbContextFactoryOptions options)
        {
            var builder = new DbContextOptionsBuilder<EDAssistantDataContext>();
            builder.UseSqlCe("Data Source=MyData.sdf;Persist Security Info=False;");
            return new EDAssistantDataContext(builder.Options);
        }
    }
#endif
}
