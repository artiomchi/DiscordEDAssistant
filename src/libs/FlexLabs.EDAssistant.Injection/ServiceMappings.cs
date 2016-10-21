using FlexLabs.EDAssistant.Repositories;
using FlexLabs.EDAssistant.Repositories.EFCompact.External.Eddb;
using FlexLabs.EDAssistant.Repositories.EFCore;
using FlexLabs.EDAssistant.Repositories.External.Eddb;
using FlexLabs.EDAssistant.Services.Commands;
using FlexLabs.EDAssistant.Services.Commands.Runners;
using FlexLabs.EDAssistant.Services.Data;
using FlexLabs.EDAssistant.Services.Integrations.Eddb;
using Microsoft.Extensions.DependencyInjection;

namespace FlexLabs.EDAssistant.Injection
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
            services.AddTransient<IKosRulesRepository, EFCoreKosRulesRepository>();
            services.AddTransient<IServersRepository, EFCoreServersRepository>();

            services.AddTransient<CommandParserService>();
            services.AddTransient<EddbDataService>();
            services.AddTransient<EddbSyncService>();
            services.AddTransient<KosRulesService>();
            services.AddTransient<ServersService>();

            // Command Runners
            services.AddTransient<EddbModuleSearchRunner>();
            services.AddTransient<EddbSystemDistanceRunner>();
            services.AddTransient<InaraWhoisRunner>();
            services.AddTransient<TimeInRunner>();
            services.AddTransient<TimeRunner>();
        }
    }
}
