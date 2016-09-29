﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FlexLabs.DiscordEDAssistant.Repositories.EFCompact.Base
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
                options.UseSqlCe(connectionString), lifetime);
        }

        public static void Init(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseSqlCe(connectionString);

            using (var context = new EDAssistantDataContext(optionsBuilder.Options))
            {
                context.Database.Migrate();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public DbSet<Server> Servers { get; set; }
    }
}