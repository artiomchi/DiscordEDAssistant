using Discord.Commands;
using FlexLabs.EDAssistant.DiscordBot.Extensions;
using FlexLabs.EDAssistant.Services.Data;
using Microsoft.Extensions.DependencyInjection;

namespace FlexLabs.EDAssistant.DiscordBot.Commands
{
    public static class SetPrefixCommands
    {
        public static void CreateCommands_SetPrefix(this CommandService commandService)
        {
            commandService.CreateCommand("setprefix")
                .Description("Set a prefix for commands")
                .Parameter("prefix", ParameterType.Optional)
                .AddCheck(Bot.Check_PublicChannel)
                .AddCheck(Bot.Check_IsServerAdmin)
                .ModCommand()
                .Do(async e =>
                {
                    var prefix = e.GetArg("prefix");
                    if (string.IsNullOrWhiteSpace(prefix)) prefix = null;
                    if (prefix?.Length > 5)
                    {
                        await e.Channel.SendMessage("Command prefix is too long");
                    }

                    using (var serversService = Bot.ServiceProvider.GetService<ServersService>())
                    {
                        serversService.SetCommandPrefix(e.Server.Id, prefix);
                    }

                    if (prefix != null)
                    {
                        Bot.ServerPrefixes[e.Server.Id] = prefix;
                        await e.Channel.SendMessage($"Command prefix set to `{prefix}`");
                    }
                    else
                    {
                        Bot.ServerPrefixes.Remove(e.Server.Id);
                        await e.Channel.SendMessage("Command prefix removed. You can @mention the bot to send commands");
                    }
                });
        }
    }
}
