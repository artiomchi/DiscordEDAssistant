using Discord;
using FlexLabs.EDAssistant.Services.Commands;
using FlexLabs.EDAssistant.Services.Data;
using System.Threading.Tasks;

namespace FlexLabs.EDAssistant.DiscordBot.Runners
{
    public class WelcomeRunner : IRunner
    {
        private readonly ServersService _serversService;
        public WelcomeRunner(ServersService serversService)
        {
            _serversService = serversService;
        }
        public void Dispose() => _serversService.Dispose();

        public string Prefix => "welcome";
        public string Template => "welcome";
        public string Title => "Display the current welcome message";

        public Task<CommandResponse> RunAsync(string[] arguments, object channelData)
        {
            var e = channelData as MessageEventArgs;

            var server = _serversService.Load(e.Server.Id);
            var welcomeMessage = server?.WelcomeMessage;
            if (welcomeMessage == null)
                return Task.FromResult(CommandResponse.Text("No welcome message set"));
            else
                return Task.FromResult(CommandResponse.Text($@"Welcome message:
{WelcomeRunnerHelper.ProcessMessageChannelLinks(e.Server, e.User, welcomeMessage)}"));
        }
    }
}
