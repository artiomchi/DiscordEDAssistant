using Discord;
using Discord.Commands;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DiscordBot
{
    class Program
    {
        static void Main(string[] args) => new Program().Start();

        private DiscordClient _client;
        private String _welcomeMessage = null;

        public void Start()
        {
            _client = new DiscordClient(x =>
            {
                x.AppName = "E:D Bot";
                x.AppVersion = "1.2.3-beta";
            });

            _client.MessageReceived += async (s, e) =>
            {
                if (e.Message.IsAuthor || !e.Channel.IsPrivate) return;
                await e.Channel.SendMessage(e.Message.Text);
            };

            _client.UsingCommands(x =>
            {
                x.PrefixChar = '~';
                x.HelpMode = HelpMode.Disabled;
            });
            var commandService = _client.GetService<CommandService>();
            commandService.CreateCommand("help")
                .Hide()
                .Parameter("Public", ParameterType.Optional)
                .Do(async e =>
                {
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
                    var maxLength = Math.Max(15, commandsHelp.Max(c => c.Name.Length + c.Arguments.Length) + 3);

                    var message = new StringBuilder();
                    message.AppendLine("Available commands:");
                    message.AppendLine("```");
                    foreach (var command in commandsHelp)
                    {
                        var padding = maxLength - command.Name.Length - command.Arguments.Length;
                        message.AppendLine($"{commandService.Config.PrefixChar}{command.Name}{command.Arguments}{new String(' ', padding)}| {command.Description}");
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
                });
            commandService.CreateCommand("greet")
                .Alias(new[] { "gr", "hi" })
                .Description("Greet a person")
                .Parameter("GreetedPerson", ParameterType.Required)
                .Do(e => e.Channel.SendMessage($"Hello {e.GetArg("GreetedPerson")}!"));

            commandService.CreateCommand("welcome")
                .Description("Set welcome message")
                .AddCheck((cmd, u, ch) => !ch.IsPrivate)
                .AddCheck((cmd, u, ch) =>
                {
                    var role = ch.Server.FindRoles("Admin", true).FirstOrDefault();
                    if (role == null)
                        role = ch.Server.FindRoles("Mod", true).FirstOrDefault();
                    if (role == null)
                        role = ch.Server.FindRoles("Neo", true).FirstOrDefault();
                    if (role == null)
                        return false;

                    return u.HasRole(role);
                })
                .Do(async e =>
                {
                    if (_welcomeMessage == null)
                        await e.Channel.SendMessage("No welcome message set");
                    else
                        await e.Channel.SendMessage($"Welcome message: {_welcomeMessage}");
                });

            commandService.CreateCommand("welcome")
                .Description("Set welcome message")
                .AddCheck((cmd, u, ch) => !ch.IsPrivate)
                .AddCheck((cmd, u, ch) =>
                {
                    var role = ch.Server.FindRoles("Admin", true).FirstOrDefault();
                    if (role == null)
                        role = ch.Server.FindRoles("Mod", true).FirstOrDefault();
                    if (role == null)
                        role = ch.Server.FindRoles("Neo", true).FirstOrDefault();
                    if (role == null)
                        return false;

                    return u.HasRole(role);
                })
                .Parameter("Message", ParameterType.Required)
                .Do(async e =>
                {
                    var message = e.GetArg("Message");
                    var channelMentions = Regex.Matches(message, @"#(\w+)\b");
                    foreach (var match in channelMentions.OfType<Match>().Select(m => m.Groups[1].Value).Distinct())
                    {
                        var channel = e.Server.FindChannels(match).FirstOrDefault();
                        if (channel != null)
                            message = Regex.Replace(message, $@"#{match}\b", channel.Mention);

                    }

                    _welcomeMessage = message;
                    await e.Channel.SendMessage("New greeting: " + message);
                });

            commandService.CreateCommand("clear-channel")
                .Hide()
                .Parameter("Password", ParameterType.Required)
                .AddCheck((cmd, u, ch) => !ch.IsPrivate)
                .AddCheck((cmd, u, ch) =>
                {
                    var role = ch.Server.FindRoles("Admin", true).FirstOrDefault();
                    if (role == null)
                        role = ch.Server.FindRoles("Mod", true).FirstOrDefault();
                    if (role == null)
                        role = ch.Server.FindRoles("Neo", true).FirstOrDefault();
                    if (role == null)
                        return false;

                    return u.HasRole(role);
                })
                .Do(async e =>
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
                });

            _client.UserJoined += (s, e) =>
            {
                if (_welcomeMessage == null) return;
                e.Server.DefaultChannel.SendMessage($"{e.User.Mention} {_welcomeMessage}");
            };

            _client.ExecuteAndWait(async () =>
            {
                await _client.Connect("MjMwNjM4MDA2ODIwNjY3Mzk0.Cs07fA.Rmkm8VS4CaA5oTjy_MHhFiV28b8", TokenType.Bot);
            });
        }
    }
}
