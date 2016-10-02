using Discord;
using Discord.Commands;
using FlexLabs.DiscordEDAssistant.Services.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace FlexLabs.DiscordEDAssistant.Bot.Commands
{
    public static class WelcomeCommands
    {
        public static void CreateCommands_Welcome(this CommandService commandService, DiscordClient client)
        {
            commandService.CreateCommand("welcome")
                .Description("Display the current welcome message")
                .AddCheck(Bot.Check_PublicChannel)
                .AddCheck(Bot.Check_IsServerAdmin)
                .Do(Command_Welcome);

            commandService.CreateCommand("welcome")
                .Description("Set a welcome message for new users")
                .Parameter("message", ParameterType.Unparsed)
                .AddCheck(Bot.Check_PublicChannel)
                .AddCheck(Bot.Check_IsServerAdmin)
                .Do(Command_Welcome_Set);

            client.UserJoined += (s, e) =>
            {
                var welcomeMessage = GetServerWelcomeMessage(e.Server.Id);
                if (welcomeMessage == null) return;
                e.Server.DefaultChannel.SendMessage(ProcessMessageChannelLinks(e.Server, e.User, welcomeMessage));
            };
        }

        private static async Task Command_Welcome(CommandEventArgs e)
        {
            var welcomeMessage = GetServerWelcomeMessage(e.Server.Id);
            if (welcomeMessage == null)
                await e.Channel.SendMessage("No welcome message set");
            else
                await e.Channel.SendMessage($@"Welcome message:
{ProcessMessageChannelLinks(e.Server, e.User, welcomeMessage)}");
        }

        private static async Task Command_Welcome_Set(CommandEventArgs e)
        {
            var welcomeMessage = e.GetArg("message");
            if (string.IsNullOrWhiteSpace(welcomeMessage) || welcomeMessage == "''" || welcomeMessage == "\"\"" || welcomeMessage == "clear") welcomeMessage = null;

            using (var serversService = Bot.ServiceProvider.GetService<ServersService>())
            {
                serversService.SetWelcomeMessage(e.Server.Id, welcomeMessage);
            }

            if (welcomeMessage != null)
                await e.Channel.SendMessage("New greeting: " + ProcessMessageChannelLinks(e.Server, e.User, welcomeMessage));
            else
                await e.Channel.SendMessage("Greeting message removed");
        }

        public static string GetServerWelcomeMessage(ulong serverID)
        {
            using (var serversService = Bot.ServiceProvider.GetService<ServersService>())
            {
                var server = serversService.Load(serverID);
                return server?.WelcomeMessage;
            }
        }

        private static string ProcessMessageChannelLinks(Server server, User user, string message)
        {
            var channelMentions = Regex.Matches(message, @"#(\w+)\b");
            foreach (var match in channelMentions.OfType<Match>().Select(m => m.Groups[1].Value).Distinct())
            {
                var channel = server.FindChannels(match).FirstOrDefault();
                if (channel != null)
                    message = Regex.Replace(message, $@"#{match}\b", channel.Mention);
            }
            message = message.IndexOf("{user}") >= 0
                ? message.Replace("{user}", user.Mention)
                : user.Mention + " " + message;
            return message;
        }

    }
}
