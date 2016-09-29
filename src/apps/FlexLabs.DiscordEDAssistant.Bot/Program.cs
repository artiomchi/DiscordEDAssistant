using FlexLabs.DiscordEDAssistant.Base.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;

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
                configBuilder.AddUserSecrets();
            configBuilder.AddEnvironmentVariables();
            var config = configBuilder.Build();

            var botToken = config["Discord.Bot.Token"];
            if (botToken == null)
            {
                Console.WriteLine("Bot auth token missing!");
                return;
            }

            var bot = new Bot();
            bot.Start(botToken);
        }
    }
}
