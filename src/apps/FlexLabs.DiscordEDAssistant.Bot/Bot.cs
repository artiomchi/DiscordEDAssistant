using Discord;
using Discord.Commands;
using FlexLabs.DiscordEDAssistant.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FlexLabs.DiscordEDAssistant.Bot
{
    public class Bot
    {
        private IServiceProvider _serviceProvider;
        private String _clientID;
        private DiscordClient _client;
        private String _welcomeMessage = "Welcome {user}! Please read the intro details in #welcome";
        private DateTime _start = DateTime.MinValue;
        private Dictionary<ulong, string> _serverPrefixes = new Dictionary<ulong, string>();

        public Bot(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _client = new DiscordClient(x =>
            {
                x.AppName = "Elite Dangerous Assistant Bot";
                x.AppVersion = Program.GetVersion();
            });

            _client.MessageReceived += async (s, e) =>
            {
                if (e.Message.IsAuthor || !e.Channel.IsPrivate) return;
                await e.Channel.SendMessage(e.Message.Text);
            };

            _client.UsingCommands(x =>
            {
                x.HelpMode = HelpMode.Disabled;
                x.CustomPrefixHandler = m =>
                {
                    if (m.Channel.IsPrivate)
                        return 0;

                    var prefix = GetServerPrefix(m.Server.Id);
                    if (prefix == null || !m.Text.StartsWith(prefix))
                        return 0;

                    return prefix.Length;
                };
            });

            var commandService = _client.GetService<CommandService>();
            commandService.CreateCommand("help")
                .Hide()
                .Parameter("Public", ParameterType.Optional)
                .Do(Command_Help);

            commandService.CreateCommand("setprefix")
                .Description("Set a prefix for commands")
                .Parameter("Prefix", ParameterType.Required)
                .AddCheck(Check_IsAdmin)
                .Do(Command_SetPrefix);

            commandService.CreateCommand("welcome")
                .Description("Set welcome message")
                .AddCheck((cmd, u, ch) => !ch.IsPrivate)
                .AddCheck(Check_IsAdmin)
                .Do(Command_Welcome);

            commandService.CreateCommand("welcome")
                .Description("Set welcome message")
                .Parameter("Message", ParameterType.Required)
                .AddCheck((cmd, u, ch) => !ch.IsPrivate)
                .AddCheck(Check_IsAdmin)
                .Do(Command_Welcome_Set);

            commandService.CreateCommand("clear-channel-history")
                .Hide()
                .Parameter("Password", ParameterType.Required)
                .AddCheck((cmd, u, ch) => !ch.IsPrivate)
                .AddCheck(Check_IsAdmin)
                .Do(Command_Clear_Channel_History);

            commandService.CreateCommand("status")
                .Alias("uptime", "about")
                .Description("Display server status")
                .Do(Command_Status);

            _client.UserJoined += (s, e) =>
            {
                if (_welcomeMessage == null) return;
                e.Server.DefaultChannel.SendMessage(ProcessMessageChannelLinks(e.Server, e.User, _welcomeMessage));
            };
        }

        public void Start(String botToken, String clientID)
        {
            _clientID = clientID;
            _start = DateTime.UtcNow;
            _client.ExecuteAndWait(async () =>
            {
                await _client.Connect(botToken, TokenType.Bot);
            });
        }

        private String GetServerPrefix(UInt64 serverID)
        {
            if (_serverPrefixes.ContainsKey(serverID))
                return _serverPrefixes[serverID];

            using (var serversService = _serviceProvider.GetService(typeof(ServersService)) as ServersService)
            {
                var server = serversService.Load(serverID);
                var commandPrefix = server?.CommandPrefix;

                if (commandPrefix != null)
                    _serverPrefixes[serverID] = commandPrefix;

                return commandPrefix;
            }
        }

        private Boolean Check_IsAdmin(Command cmd, User u, Channel ch)
        {
            var role = ch.Server.FindRoles("Admin", true).FirstOrDefault();
            if (role == null)
                role = ch.Server.FindRoles("Mod", true).FirstOrDefault();
            if (role == null)
                role = ch.Server.FindRoles("Neo", true).FirstOrDefault();
            if (role == null)
                return false;

            return u.HasRole(role);
        }

        private async Task Command_Help(CommandEventArgs e)
        {
            var commandService = _client.GetService<CommandService>();
            var commands = commandService.AllCommands.OfType<Command>().Where(c => !c.IsHidden);
            var commandsHelp = commands.Select(command =>
            {
                var arguments = command.Parameters.Any()
                    ? " " + String.Join(" ", command.Parameters.Select(a => $"{{{a.Name}}}"))
                    : String.Empty;
                return new
                {
                    Arguments = arguments,
                    Name = command.Text,
                    Description = command.Description,
                };
            });
            var maxLength = Math.Max(10, commandsHelp.Max(c => c.Name.Length + c.Arguments.Length) + 1);

            var prefix = GetServerPrefix(e.Server.Id);
            var message = new StringBuilder();
            message.AppendLine("Available commands:");
            message.AppendLine("```http");
            foreach (var command in commandsHelp)
            {
                var padding = maxLength - command.Name.Length - command.Arguments.Length;
                message.AppendLine($"{prefix}{command.Name}{command.Arguments}{new String(' ', padding)}: {command.Description}");
            }
            message.AppendLine("```");

            var pub = e.Args.Length > 0
                ? String.Equals(e.GetArg("Public"), "here", StringComparison.OrdinalIgnoreCase)
                : false;
            if (e.Channel.IsPrivate || pub)
            {
                await e.Channel.SendMessage(message.ToString());
            }
            else
            {
                await e.User.SendMessage(message.ToString());
                await e.Channel.SendMessage($"{e.User.Mention} Bot instructions have been sent to you as a Direct Message");
            }
        }

        private async Task Command_SetPrefix(CommandEventArgs e)
        {
            var prefix = e.GetArg("Prefix");

            using (var serversService = _serviceProvider.GetService(typeof(ServersService)) as ServersService)
            {
                serversService.SetCommandPrefix(e.Server.Id, prefix);
            }
            await e.Channel.SendMessage("Prefix updated");
        }

        private String ProcessMessageChannelLinks(Server server, User user, String message)
        {
            var channelMentions = Regex.Matches(message, @"#(\w+)\b");
            foreach (var match in channelMentions.OfType<Match>().Select(m => m.Groups[1].Value).Distinct())
            {
                var channel = server.FindChannels(match).FirstOrDefault();
                if (channel != null)
                    message = Regex.Replace(message, $@"#{match}\b", channel.Mention);
            }
            return message.Replace("{user}", user.Mention);
        }

        private async Task Command_Welcome(CommandEventArgs e)
        {
            if (_welcomeMessage == null)
                await e.Channel.SendMessage("No welcome message set");
            else
                await e.Channel.SendMessage($"Welcome message: {ProcessMessageChannelLinks(e.Server, e.User, _welcomeMessage)}");
        }

        private async Task Command_Welcome_Set(CommandEventArgs e)
        {
            _welcomeMessage = e.GetArg("Message");
            await e.Channel.SendMessage("New greeting: " + ProcessMessageChannelLinks(e.Server, e.User, _welcomeMessage));
        }

        private async Task Command_Clear_Channel_History(CommandEventArgs e)
        {
            var pass = e.GetArg("Password");
            if (pass != "passw0rd!")
            {
                await e.Channel.SendMessage("Password invalid!");
                return;
            }

            Message[] messages;
            while ((messages = await e.Channel.DownloadMessages()).Length > 0)
                await e.Channel.DeleteMessages(messages);
        }

        private async Task Command_Status(CommandEventArgs e)
        {
            var message = 
$@"```
Status:             OK
Current location:   {Environment.MachineName}
Uptime:             {DateTime.UtcNow.Subtract(_start).ToString()}
Build:              {Program.GetVersion()}
Build time:         {Program.GetBuildTime()}
```";
            if (_clientID != null)
                message+= $@"
If you want to add this bot to your server, follow this link:
https://discordapp.com/oauth2/authorize?client_id={_clientID}&scope=bot&permissions=3072";

            await e.Channel.SendMessage(message);
        }
    }
}
