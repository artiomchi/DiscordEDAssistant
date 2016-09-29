using FlexLabs.DiscordEDAssistant.Base.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using FlexLabs.DiscordEDAssistant.Injection;

namespace FlexLabs.DiscordEDAssistant.Bot
{
    public class Program
    {
        public static void Main() => new Program().Start();

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

        public void Start()
        {
            var configBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            if (System.Diagnostics.Debugger.IsAttached)
                configBuilder.AddUserSecrets("aspnet-FlexLabs.DiscordEDAssistant-20160929030746");
            configBuilder.AddEnvironmentVariables();
            var config = configBuilder.Build();

            var botToken = config.GetConnectionString("Discord.Bot.Token");
            if (botToken == null)
            {
                Console.WriteLine("Bot auth token missing!");
                return;
            }

            var dbConnectionString = config.GetConnectionString("DefaultConnection");

            var services = new ServiceCollection();
            ServiceMappings.ConfigureDatabase(services, dbConnectionString);
            ServiceMappings.ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();

            ServiceMappings.InitDatabase(dbConnectionString);

            var bot = new Bot(serviceProvider);
            bot.Start(botToken, config["Discord.Bot.ClientID"]);
        }
    }
}
