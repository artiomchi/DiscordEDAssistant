using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FlexLabs.DiscordEDAssistant.Repositories.EFCore.Base
{
    public class EDAssistantDataContext : DbContext
    {
        internal const int LongTimeoutMs = 30000000;

        public EDAssistantDataContext(DbContextOptions options)
            : base(options)
        {
        }

        public static void Configure(IServiceCollection services, ServiceLifetime lifetime, string connectionString)
        {
            ConnectionString = connectionString;
            services.AddDbContext<EDAssistantDataContext>(options =>
                options.UseSqlServer(connectionString), lifetime);
        }

        public static void Init(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseSqlServer(connectionString);

            using (var context = new EDAssistantDataContext(optionsBuilder.Options))
            {
                context.Database.SetCommandTimeout(LongTimeoutMs);
                context.Database.Migrate();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        internal static string ConnectionString { get; private set; }

        public DbSet<KosSimpleRule> KosSimpleRules { get; set; }
        public DbSet<KosUserRule> KosUserRules { get; set; }
        public DbSet<Server> Servers { get; set; }
        public DbSet<Eddb_Commodity> Eddb_Commodities { get; set; }
        public DbSet<Eddb_Commodities_Category> Eddb_Commodities_Categories { get; set; }
        public DbSet<Eddb_Module> Eddb_Modules { get; set; }
        public DbSet<Eddb_Modules_Category> Eddb_Modules_Categories { get; set; }
        public DbSet<Eddb_Modules_Group> Eddb_Modules_Groups { get; set; }
        public DbSet<Eddb_StarSystem> Eddb_StarSystems { get; set; }
        public DbSet<Eddb_Station> Eddb_Stations { get; set; }
        public DbSet<Eddb_Stations_SellingModule> Eddb_Stations_SellingModules { get; set; }
        public DbSet<Eddb_Stations_SellingShip> Eddb_Stations_SellingShips { get; set; }
        public DbSet<Eddb_Stations_Type> Eddb_Stations_Types { get; set; }
        public DbSet<Upload_Eddb_Commodity> Upload_Eddb_Commodities { get; set; }
        public DbSet<Upload_Eddb_Module> Upload_Eddb_Modules { get; set; }
        public DbSet<Upload_Eddb_StarSystem> Upload_Eddb_StarSystems { get; set; }
        public DbSet<Upload_Eddb_Station> Upload_Eddb_Stations { get; set; }
    }
}
