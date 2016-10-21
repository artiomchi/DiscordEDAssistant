using Discord;
using FlexLabs.EDAssistant.Services.Commands;
using FlexLabs.EDAssistant.Services.Data;
using System.Threading.Tasks;

namespace FlexLabs.EDAssistant.DiscordBot.Runners
{
    public class WelcomeSetRunner : IRunner
    {
        private readonly ServersService _serversService;
        public WelcomeSetRunner(ServersService serversService)
        {
            _serversService = serversService;
        }
        public void Dispose() => _serversService.Dispose();

        public string Prefix => "welcome";
        public string Template => "welcome set {message}";
        public string Title => "Set a welcome message for new users";

        public Task<CommandResponse> RunAsync(string[] arguments, object channelData)
        {
            var e = channelData as MessageEventArgs;

            var welcomeMessage = arguments[0];
            if (string.IsNullOrWhiteSpace(welcomeMessage) || welcomeMessage == "''" || welcomeMessage == "\"\"" || welcomeMessage == "clear") welcomeMessage = null;

            _serversService.SetWelcomeMessage(e.Server.Id, welcomeMessage);

            if (welcomeMessage != null)
                return Task.FromResult(CommandResponse.Text("New greeting: " + WelcomeRunnerHelper.ProcessMessageChannelLinks(e.Server, e.User, welcomeMessage)));
            else
                return Task.FromResult(CommandResponse.Text("Greeting message removed"));
        }
    }
}
