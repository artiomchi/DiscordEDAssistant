using FlexLabs.EDAssistant.Base.Extensions;
using FlexLabs.EDAssistant.Injection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Reflection;

namespace FlexLabs.EDAssistant.DiscordBot
{
    public class Program
    {
        public static void Main(string[] args) => new Program().Start(args);

        public static string GetVersion()
        {
            var assembly = typeof(Program).GetTypeInfo().Assembly;
            var version = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
            if (version == null)
                version = assembly.GetName().Version.ToString();
            return version;
        }

        public static DateTime GetBuildTime()
        {
            var assembly = typeof(Program).GetTypeInfo().Assembly;
            return assembly.GetName().Version.AsDateTime();
        }

        public void Start(string[] args)
        {
            var configBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            if (System.Diagnostics.Debugger.IsAttached)
                configBuilder.AddUserSecrets("aspnet-FlexLabs.EDAssistant-20160929030746");
            configBuilder.AddEnvironmentVariables(Models.Settings.EnvironmentPrefix);
            var config = configBuilder.Build();

            var dbConnectionString = config.GetConnectionString("DefaultConnection");
            var services = new ServiceCollection();
            services.AddOptions();
            services.Configure<Models.Settings>(config);
            ServiceMappings.ConfigureDatabase(services, dbConnectionString);
            ServiceMappings.ConfigureServices(services);
            services.AddTransient<Bot>();
            services.AddTransient<Runners.KosRunner>();
            services.AddTransient<Runners.KosSetRunner>();
            services.AddTransient<Runners.KosRemoveRunner>();
            services.AddTransient<Runners.WelcomeRunner>();
            services.AddTransient<Runners.WelcomeSetRunner>();
            services.AddTransient<Runners.SetPrefixRunner>();
            services.AddTransient<Runners.AboutRunner>();
            var serviceProvider = services.BuildServiceProvider();
            ServiceMappings.InitDatabase(dbConnectionString);

            var settings = serviceProvider.GetService<IOptions<Models.Settings>>().Value;

            if (settings.Discord.AuthToken == null)
            {
                Console.WriteLine("Bot auth token missing!");
                return;
            }

            switch (args?.Length > 0 ? args[0] : null)
            {
                case "eddb.sync":
                    using (var syncService = serviceProvider.GetService<Services.Integrations.Eddb.EddbSyncService>())
                    {
                        syncService.SyncAsync().Wait();
                    }
                    break;
                default:

                    var bot = serviceProvider.GetService<Bot>();
                    bot.Start();
                    break;
            }
        }
    }
}
