using FlexLabs.DiscordEDAssistant.Repositories;
using FlexLabs.DiscordEDAssistant.Repositories.EFCompact.External.Eddb;
using FlexLabs.DiscordEDAssistant.Repositories.EFCore;
using FlexLabs.DiscordEDAssistant.Repositories.External.Eddb;
using FlexLabs.DiscordEDAssistant.Services.Data;
using FlexLabs.DiscordEDAssistant.Services.Integrations.Eddb;
using Microsoft.Extensions.DependencyInjection;

namespace FlexLabs.DiscordEDAssistant.Injection
{
    public static class ServiceMappings
    {
        public static void ConfigureDatabase(IServiceCollection services, string connectionString)
            => Repositories.EFCore.Base.EDAssistantDataContext.Configure(services, ServiceLifetime.Transient, connectionString);

        public static void InitDatabase(string connectionString)
            => Repositories.EFCore.Base.EDAssistantDataContext.Init(connectionString);

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IEddbDataRepository, EFCoreEddbDataRepository>();
            services.AddTransient<IEddbUpdateRepository, EFCoreEddbUpdateRepository>();
            services.AddTransient<IServersRepository, EFCoreServersRepository>();

            services.AddTransient<EddbDataService>();
            services.AddTransient<EddbSyncService>();
            services.AddTransient<ServersService>();
        }
    }
}
