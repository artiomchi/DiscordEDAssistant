using FlexLabs.DiscordEDAssistant.Repositories;
using FlexLabs.DiscordEDAssistant.Repositories.EFCompact;
using FlexLabs.DiscordEDAssistant.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FlexLabs.DiscordEDAssistant.Injection
{
    public static class ServiceMappings
    {
        public static void ConfigureDatabase(IServiceCollection services, string connectionString)
            => Repositories.EFCompact.Base.EDAssistantDataContext.Configure(services, ServiceLifetime.Transient, connectionString);

        public static void InitDatabase(string connectionString)
            => Repositories.EFCompact.Base.EDAssistantDataContext.Init(connectionString);

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IServersRepository, EFCompactServersRepository>();

            services.AddTransient<ServersService>();
        }
    }
}
