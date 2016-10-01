using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FlexLabs.DiscordEDAssistant.Repositories.EFCore.Base
{
    public class EDAssistantDataContext : DbContext
    {
        public EDAssistantDataContext(DbContextOptions options)
            : base(options)
        {
        }

        public static void Configure(IServiceCollection services, ServiceLifetime lifetime, string connectionString)
        {
            services.AddDbContext<EDAssistantDataContext>(options =>
                options.UseSqlServer(connectionString), lifetime);
        }

        public static void Init(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseSqlServer(connectionString);

            using (var context = new EDAssistantDataContext(optionsBuilder.Options))
            {
                context.Database.Migrate();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public DbSet<Server> Servers { get; set; }
        public DbSet<Eddb_Module> Eddb_Modules { get; set; }
        public DbSet<Eddb_Modules_Category> Eddb_Modules_Categories { get; set; }
        public DbSet<Eddb_Modules_Group> Eddb_Modules_Groups { get; set; }
        public DbSet<Eddb_System> Eddb_Systems { get; set; }
        public DbSet<Upload_Eddb_Module> Upload_Eddb_Modules { get; set; }
        public DbSet<Upload_Eddb_System> Upload_Eddb_Systems { get; set; }
    }
}
