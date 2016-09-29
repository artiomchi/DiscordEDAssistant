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
        private string _clientID;
        private DiscordClient _client;
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

            _client.UsingCommands(x =>
            {
                x.HelpMode = HelpMode.Disabled;
                x.CustomPrefixHandler = m =>
                {
                    if (m.Channel.IsPrivate || m.Server == null) return 0;

                    var prefix = GetServerPrefix(m.Server.Id);
                    if (prefix == null || !m.Text.StartsWith(prefix))
                        return -1;

                    return prefix.Length;
                };
            });

            var commandService = _client.GetService<CommandService>();
            commandService.CreateCommand("help")
                .Hide()
                .Parameter("public", ParameterType.Optional)
                .Do(Command_Help);

            commandService.CreateCommand("setprefix")
                .Description("Set a prefix for commands")
                .Parameter("prefix", ParameterType.Optional)
                .AddCheck(Check_PublicChannel)
                .AddCheck(Check_IsServerAdmin)
                .Do(Command_SetPrefix);

            commandService.CreateCommand("welcome")
                .Description("Display the current welcome message")
                .AddCheck(Check_PublicChannel)
                .AddCheck(Check_IsServerAdmin)
                .Do(Command_Welcome);

            commandService.CreateCommand("welcome")
                .Description("Set a welcome message for new users")
                .Parameter("message", ParameterType.Unparsed)
                .AddCheck(Check_PublicChannel)
                .AddCheck(Check_IsServerAdmin)
                .Do(Command_Welcome_Set);

            commandService.CreateGroup("time", x =>
            {
                x.CreateCommand("")
                    .Description("Display the in-game time (UTC)")
                    .Do(Command_Time);

                x.CreateCommand("in")
                    .Description("Convert in-game time to local time")
                    .Parameter("timezone", ParameterType.Required)
                    .Parameter("time", ParameterType.Optional)
                    .Do(Command_TimeIn);
            });

            commandService.CreateCommand("about")
                .Alias("uptime", "status")
                .Description("Display server information")
                .Do(Command_Status);

            commandService.CreateCommand("clear-channel-history")
                .Hide()
                .Parameter("password", ParameterType.Required)
                .AddCheck(Check_PublicChannel)
                .AddCheck(Check_IsServerAdmin)
                .Do(Command_Clear_Channel_History);

            _client.UserJoined += (s, e) =>
            {
                var welcomeMessage = GetServerWelcomeMessage(e.Server.Id);
                if (welcomeMessage == null) return;
                e.Server.DefaultChannel.SendMessage(ProcessMessageChannelLinks(e.Server, e.User, welcomeMessage));
            };
        }

        public void Start(string botToken, string clientID)
        {
            _clientID = clientID;
            _start = DateTime.UtcNow;
            _client.ExecuteAndWait(async () =>
            {
                await _client.Connect(botToken, TokenType.Bot);
            });
        }

        private string GetServerPrefix(ulong serverID)
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

        private string GetServerWelcomeMessage(ulong serverID)
        {
            using (var serversService = _serviceProvider.GetService(typeof(ServersService)) as ServersService)
            {
                var server = serversService.Load(serverID);
                return server?.WelcomeMessage;
            }
        }

        private bool Check_PublicChannel(Command cmd, User u, Channel ch) => !ch.IsPrivate;
        private bool Check_IsServerAdmin(Command cmd, User u, Channel ch) => !ch.IsPrivate && u.ServerPermissions.Administrator;

        private async Task Command_Help(CommandEventArgs e)
        {
            var commandService = _client.GetService<CommandService>();
            var commands = commandService.AllCommands.OfType<Command>()
                .Where(c => !c.IsHidden)
                .Where(c =>
                {
                    var method = typeof(Command).GetMethod("CanRun", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    if (method != null)
                    {
                        try
                        {
                            var canRun = (bool)method.Invoke(c, new object[] { e.User, e.Channel, null });
                            return canRun;
                        }
                        catch { }
                    }
                    return true;
                });
            var commandsHelp = commands.Select(command =>
            {
                var arguments = command.Parameters.Any()
                    ? " " + string.Join(" ", command.Parameters.Select(a => $"{{{a.Name}}}"))
                    : string.Empty;
                return new
                {
                    Arguments = arguments,
                    Name = command.Text,
                    Description = command.Description,
                };
            });
            var maxLength = Math.Max(10, commandsHelp.Max(c => c.Name.Length + c.Arguments.Length) + 1);

            var prefix = e.Server != null ? GetServerPrefix(e.Server.Id) : null;
            var message = new StringBuilder();
            message.AppendLine($"Available commands for {e.Server?.Name ?? "direct messages"}:");
            message.AppendLine("```http");
            foreach (var command in commandsHelp)
            {
                var padding = maxLength - command.Name.Length - command.Arguments.Length;
                message.AppendLine($"{prefix}{command.Name}{command.Arguments}{new string(' ', padding)}: {command.Description}");
            }
            message.AppendLine("```");

            var pub = e.Args.Length > 0
                ? string.Equals(e.GetArg("public"), "here", StringComparison.OrdinalIgnoreCase)
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
            var prefix = e.GetArg("prefix");
            if (string.IsNullOrWhiteSpace(prefix)) prefix = null;
            if (prefix?.Length > 5)
            {
                await e.Channel.SendMessage("Command prefix is too long");
            }

            using (var serversService = _serviceProvider.GetService(typeof(ServersService)) as ServersService)
            {
                serversService.SetCommandPrefix(e.Server.Id, prefix);
            }

            if (prefix != null)
                await e.Channel.SendMessage($"Command prefix set to `{prefix}`");
            else
                await e.Channel.SendMessage("Command prefix removed. You can @mention the bot to send commands");
        }

        private string ProcessMessageChannelLinks(Server server, User user, string message)
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

        private async Task Command_Welcome(CommandEventArgs e)
        {
            var welcomeMessage = GetServerWelcomeMessage(e.Server.Id);
            if (welcomeMessage == null)
                await e.Channel.SendMessage("No welcome message set");
            else
                await e.Channel.SendMessage($@"Welcome message:
{ProcessMessageChannelLinks(e.Server, e.User, welcomeMessage)}");
        }

        private async Task Command_Welcome_Set(CommandEventArgs e)
        {
            var welcomeMessage = e.GetArg("message");
            if (string.IsNullOrWhiteSpace(welcomeMessage) || welcomeMessage == "''" || welcomeMessage == "\"\"" || welcomeMessage == "clear") welcomeMessage = null;

            using (var serversService = _serviceProvider.GetService(typeof(ServersService)) as ServersService)
            {
                serversService.SetWelcomeMessage(e.Server.Id, welcomeMessage);
            }

            if (welcomeMessage != null)
                await e.Channel.SendMessage("New greeting: " + ProcessMessageChannelLinks(e.Server, e.User, welcomeMessage));
            else
                await e.Channel.SendMessage("Greeting message removed");
        }

        private async Task Command_Clear_Channel_History(CommandEventArgs e)
        {
            var pass = e.GetArg("password");
            if (pass != "passw0rd!")
            {
                await e.Channel.SendMessage("Password invalid!");
                return;
            }

            Message[] messages;
            while ((messages = await e.Channel.DownloadMessages()).Length > 0)
                await e.Channel.DeleteMessages(messages);
        }

        private Task Command_Time(CommandEventArgs e)
            => e.Channel.SendMessage($"Current in-game time: `{FormatTime(DateTime.UtcNow)}` (UTC)");

        private async Task Command_TimeIn(CommandEventArgs e)
        {
            var timeZoneName = e.GetArg("timezone");
            var timeZone = GetTimeZone(timeZoneName);
            if (timeZone == null)
            {
                await e.Channel.SendMessage("Could not understand the time zone name");
                return;
            }

            DateTime time;
            var customTime = false;
            if (!string.IsNullOrWhiteSpace(e.GetArg("time")))
            {
                customTime = true;
                if (!DateTime.TryParse(e.GetArg("time"), out time))
                {
                    await e.Channel.SendMessage("Could not understand the time");
                    return;
                }
            }
            else
            {
                time = DateTime.UtcNow;
            }

            var newTime = TimeZoneInfo.ConvertTimeFromUtc(time, timeZone);
            if (customTime)
                await e.Channel.SendMessage($"The time in `{timeZoneName}` at `{FormatTime(time)}` UTC will be `{FormatTime(newTime)}`");
            else
                await e.Channel.SendMessage($"The time in `{timeZoneName}` is `{FormatTime(newTime)}`");
        }

        private String FormatTime(DateTime time) => time.ToString("HH:mm:ss");

        private TimeZoneInfo GetTimeZone(String timeZoneName)
        {
            switch (timeZoneName.ToLowerInvariant())
            {
                case "pdt":
                case "pst": return TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
                case "cdt":
                case "cst": return TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
                case "edt":
                case "est": return TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                case "bst":
                case "gmt": return TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
                case "utc": return TimeZoneInfo.FindSystemTimeZoneById("UTC");
                case "cet": return TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time");
                case "eet": return TimeZoneInfo.FindSystemTimeZoneById("E. Europe Standard Time");
                case "ch": return TimeZoneInfo.FindSystemTimeZoneById("China Standard Time");
                case "aus": return TimeZoneInfo.FindSystemTimeZoneById("AUS Central Standard Time");
                case "eaus": return TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time");
                default:
                    try
                    {
                        return TimeZoneInfo.FindSystemTimeZoneById(timeZoneName);
                    }
                    catch
                    {
                        return null;
                    }
            }
        }

        private async Task Command_Status(CommandEventArgs e)
        {
            var message = 
$@"```http
Status           : OK
Current location : {Environment.MachineName}
Uptime           : {DateTime.UtcNow.Subtract(_start).ToString()}
Build            : {Program.GetVersion()}
Build time       : {Program.GetBuildTime()}
```";
            if (_clientID != null)
                message+= $@"
If you want to add this bot to your server, follow this link:
https://discordapp.com/oauth2/authorize?client_id={_clientID}&scope=bot&permissions=3072";

            await e.Channel.SendMessage(message);
        }
    }
}
