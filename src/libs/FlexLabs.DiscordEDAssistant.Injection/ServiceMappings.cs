using FlexLabs.DiscordEDAssistant.Repositories;
using FlexLabs.DiscordEDAssistant.Repositories.EFCore;
using FlexLabs.DiscordEDAssistant.Services.Data;
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
            services.AddTransient<IServersRepository, EFCoreServersRepository>();

            services.AddTransient<ServersService>();
        }
    }
}
