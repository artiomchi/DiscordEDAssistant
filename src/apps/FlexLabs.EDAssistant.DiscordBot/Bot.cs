using Discord;
using FlexLabs.EDAssistant.Services.Commands;
using FlexLabs.EDAssistant.Services.Data;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;

namespace FlexLabs.EDAssistant.DiscordBot
{
    public class Bot
    {
        public static DiscordClient Client { get; private set; }
        private readonly CommandParserService commandParserService;

        public static IServiceProvider ServiceProvider { get; private set; }
        public static DateTime Started { get; private set; }
        public static Dictionary<ulong, string> ServerPrefixes { get; } = new Dictionary<ulong, string>();

        public Bot(IServiceProvider serviceProvider, CommandParserService commandParserService)
        {
            ServiceProvider = serviceProvider;
            Client = new DiscordClient(x =>
            {
                x.AppName = "Elite Dangerous Assistant Bot";
                x.AppVersion = Program.GetVersion();
            });

            Client.MessageReceived += async (s, e) =>
            {
                var message = e.Message.Text;
                var prefix = e.Server != null
                    ? GetServerPrefix(e.Server.Id)
                    : "/";

                if (prefix != null && message.StartsWith(prefix))
                    message = message.Substring(prefix.Length);
                else if (message.StartsWith(Client.CurrentUser.Mention))
                    message = message.Substring(Client.CurrentUser.Mention.Length).Trim();
                else if (message.StartsWith(Client.CurrentUser.NicknameMention))
                    message = message.Substring(Client.CurrentUser.NicknameMention.Length).Trim();
                else
                    return;

                using (var timer = new Timer(delegate { e.Channel.SendIsTyping(); }, null, 0, 3000))
                {
                    var result = await commandParserService.ProcessCommandAsync("discord", message, e);
                    if (result.Contents?.Count > 0)
                        foreach (var content in result.Contents)
                            await e.Channel.SendMessage(content.Format("discord"));
                }
            };

            Client.UserJoined += async (s, e) =>
            {
                using (var serversService = ServiceProvider.GetService<ServersService>())
                {
                    var welcomeMessage = serversService.Load(e.Server.Id)?.WelcomeMessage;
                    if (welcomeMessage == null)
                        return;
                    await e.Server.DefaultChannel.SendMessage(Runners.WelcomeRunnerHelper.ProcessMessageChannelLinks(e.Server, e.User, welcomeMessage));
                }
            };

            commandParserService.RegisterRunner<Runners.KosRunner>();
            commandParserService.RegisterRunner<Runners.KosSetRunner>();
            commandParserService.RegisterRunner<Runners.KosRemoveRunner>();
            commandParserService.RegisterRunner<Runners.WelcomeRunner>();
            commandParserService.RegisterRunner<Runners.WelcomeSetRunner>();
            commandParserService.RegisterRunner<Runners.SetPrefixRunner>();
            commandParserService.RegisterRunner<Runners.AboutRunner>();
        }

        public void Start()
        {
            Started = DateTime.UtcNow;
            Client.ExecuteAndWait(async () =>
            {
                await Client.Connect(Models.Settings.Instance.Discord.AuthToken, TokenType.Bot);
            });
        }

        private string GetServerPrefix(ulong serverID)
        {
            if (ServerPrefixes.ContainsKey(serverID))
                return ServerPrefixes[serverID];

            using (var serversService = ServiceProvider.GetService(typeof(ServersService)) as ServersService)
            {
                var server = serversService.Load(serverID);
                var commandPrefix = server?.CommandPrefix;

                if (commandPrefix != null)
                    ServerPrefixes[serverID] = commandPrefix;

                return commandPrefix;
            }
        }

        //private async Task Command_Help(CommandEventArgs e)
        //{
        //    var all = (e.Channel.IsPrivate || e.User.ServerPermissions.Administrator) && e.Args?.Length > 0 && e.Args.Any(a => String.Equals("all", a, StringComparison.OrdinalIgnoreCase));
        //    var reveal = e.Channel.IsPrivate && e.Args?.Length > 0 && e.Args.Any(a => String.Equals("reveal", a, StringComparison.OrdinalIgnoreCase));

        //    var commandService = Client.GetService<CommandService>();
        //    var commands = commandService.AllCommands.OfType<Command>()
        //        .Where(c => reveal || !c.IsHidden)
        //        .Where(c => all || !c.IsModCommand())
        //        .Where(c =>
        //        {
        //            var method = typeof(Command).GetMethod("CanRun", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        //            if (method != null)
        //            {
        //                try
        //                {
        //                    var canRun = (bool)method.Invoke(c, new object[] { e.User, e.Channel, null });
        //                    return canRun;
        //                }
        //                catch { }
        //            }
        //            return true;
        //        });
        //    var commandsHelp = commands.Select(command =>
        //    {
        //        var arguments = command.Parameters.Any()
        //            ? " " + string.Join(" ", command.Parameters.Select(a => $"{{{a.Name}}}"))
        //            : string.Empty;
        //        return new
        //        {
        //            Arguments = arguments,
        //            Name = command.Text,
        //            Description = command.Description,
        //        };
        //    });
        //    var maxLength = Math.Max(10, commandsHelp.Max(c => c.Name.Length + c.Arguments.Length) + 1);

        //    var prefix = e.Server != null ? GetServerPrefix(e.Server.Id) : null;
        //    var message = new StringBuilder();
        //    message.AppendLine(!e.Channel.IsPrivate && e.User.ServerPermissions.Administrator && !all
        //        ? $"Popular commands for {e.Server?.Name ?? "direct messages"} (to include mod commands run `{prefix}help all`):"
        //        : $"Available commands for {e.Server?.Name ?? "direct messages"}:");
        //    message.AppendLine("```http");
        //    foreach (var command in commandsHelp)
        //    {
        //        var padding = maxLength - command.Name.Length - command.Arguments.Length;
        //        message.AppendLine($"{prefix}{command.Name}{command.Arguments}{new string(' ', padding)}: {command.Description}");
        //    }
        //    message.AppendLine("```");

        //    await e.Channel.SendMessage(message.ToString());
        //}
    }
}
