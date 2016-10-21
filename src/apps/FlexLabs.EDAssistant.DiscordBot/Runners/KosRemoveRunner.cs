using Discord;
using FlexLabs.EDAssistant.Services.Commands;
using FlexLabs.EDAssistant.Services.Data;
using System.Threading.Tasks;

namespace FlexLabs.EDAssistant.DiscordBot.Runners
{
    public class KosRemoveRunner : IRunner
    {
        private readonly KosRulesService _rulesService;
        public KosRemoveRunner(KosRulesService rulesService)
        {
            _rulesService = rulesService;
        }
        public void Dispose() => _rulesService.Dispose();

        public string Prefix => "kos remove";
        public string Template => "kos remove {name}";
        public string Title => "Remove KOS rules for user";

        public async Task<CommandResponse> RunAsync(string[] arguments, object channelData)
        {
            var e = channelData as MessageEventArgs;
            var user = arguments[0];

            await _rulesService.DeleteAsync(e.Server.Id, user);

            return CommandResponse.Text("KOS removed");
        }
    }
}
