using FlexLabs.EDAssistant.Base.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using FlexLabs.EDAssistant.Injection;

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

        public static IConfigurationRoot Config { get; private set; }
        public void Start(string[] args)
        {
            var configBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            if (System.Diagnostics.Debugger.IsAttached)
                configBuilder.AddUserSecrets("aspnet-FlexLabs.EDAssistant-20160929030746");
            configBuilder.AddEnvironmentVariables();
            Config = configBuilder.Build();

            var botToken = Config["Discord:Bot.Token"];
            if (botToken == null)
            {
                Console.WriteLine("Bot auth token missing!");
                return;
            }

            var dbConnectionString = Config.GetConnectionString("DefaultConnection");

            var services = new ServiceCollection();
            ServiceMappings.ConfigureDatabase(services, dbConnectionString);
            ServiceMappings.ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();

            ServiceMappings.InitDatabase(dbConnectionString);



            switch (args?.Length > 0 ? args[0] : null)
            {
                case "eddb.sync":
                    using (var syncService = serviceProvider.GetService<Services.Integrations.Eddb.EddbSyncService>())
                    {
                        syncService.SyncAsync().Wait();
                    }
                    break;
                default:
                    var bot = new Bot(serviceProvider);
                    bot.Start(botToken, Config["Discord:Bot.ClientID"]);
                    break;
            }
        }
    }
}
