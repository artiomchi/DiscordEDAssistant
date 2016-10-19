using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace FlexLabs.EDAssistant.Repositories.EFCore.Base
{
#if DEBUG
    public class EDAssistantDataContextFactory : IDbContextFactory<EDAssistantDataContext>
    {
        public EDAssistantDataContext Create(DbContextFactoryOptions options)
        {
            var builder = new DbContextOptionsBuilder<EDAssistantDataContext>();
            builder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=EDAssistant_db;Trusted_Connection=True;");
            return new EDAssistantDataContext(builder.Options);
        }
    }
#endif
}
