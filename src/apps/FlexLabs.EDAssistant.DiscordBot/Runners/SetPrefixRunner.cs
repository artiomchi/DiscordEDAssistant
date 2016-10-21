using Discord;
using FlexLabs.EDAssistant.Services.Commands;
using FlexLabs.EDAssistant.Services.Data;
using System.Threading.Tasks;

namespace FlexLabs.EDAssistant.DiscordBot.Runners
{
    public class SetPrefixRunner : IRunner
    {
        private readonly ServersService _serversService;
        public SetPrefixRunner(ServersService serversService)
        {
            _serversService = serversService;
        }
        public void Dispose() => _serversService.Dispose();

        public string Prefix => "setprefix";
        public string Template => "setprefix {prefix}";
        public string Title => "Set a prefix for commands";

        public Task<CommandResponse> RunAsync(string[] arguments, object channelData)
        {
            var e = channelData as MessageEventArgs;
            var prefix = arguments[0];
            if (string.IsNullOrWhiteSpace(prefix)) prefix = null;
            if (prefix?.Length > 5)
                return Task.FromResult(CommandResponse.Error("Command prefix is too long"));

            _serversService.SetCommandPrefix(e.Server.Id, prefix);

            if (prefix != null)
            {
                Bot.ServerPrefixes[e.Server.Id] = prefix;
                return Task.FromResult(CommandResponse.Text($"Command prefix set to `{prefix}`"));
            }
            else
            {
                Bot.ServerPrefixes.Remove(e.Server.Id);
                return Task.FromResult(CommandResponse.Text("Command prefix removed. You can @mention the bot to send commands"));
            }
        }
    }
}
