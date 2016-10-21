using FlexLabs.EDAssistant.Services.Commands;
using FlexLabs.EDAssistant.Services.Data;
using System.Threading.Tasks;
using Discord;

namespace FlexLabs.EDAssistant.DiscordBot.Runners
{
    public class KosSetRunner : IRunner
    {
        private readonly KosRulesService _rulesService;
        public KosSetRunner(KosRulesService rulesService)
        {
            _rulesService = rulesService;
        }
        public void Dispose() => _rulesService.Dispose();

        public string Prefix => "kos set";
        public string Template => "kos set {name} {rule}";
        public string Title => "Set KOS rule for user";

        public async Task<CommandResponse> RunAsync(string[] arguments, object channelData)
        {
            var e = channelData as MessageEventArgs;
            var user = arguments[0];
            var rule = arguments[1];

            await _rulesService.SetAsync(e.Server.Id, user, e.User.Id, rule);

            return CommandResponse.Text("KOS set");
        }
    }
}
