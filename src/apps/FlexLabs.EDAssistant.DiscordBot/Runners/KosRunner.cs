using FlexLabs.EDAssistant.Services.Commands;
using FlexLabs.EDAssistant.Services.Data;
using System.Threading.Tasks;
using Discord;

namespace FlexLabs.EDAssistant.DiscordBot.Runners
{
    public class KosRunner : IRunner
    {
        private readonly KosRulesService _rulesService;
        public KosRunner(KosRulesService rulesService)
        {
            _rulesService = rulesService;
        }
        public void Dispose() => _rulesService.Dispose();

        public string Prefix => "kos";
        public string Template => "kos {name}";
        public string Title => "Check the status of KOS for a user";

        public async Task<CommandResponse> RunAsync(string[] arguments, object channelData)
        {
            var e = channelData as MessageEventArgs;
            var user = arguments[0];
            var rule = await _rulesService.LoadAsync(e.Server.Id, user);

            if (rule != null)
                return CommandResponse.Text($"Yes, {rule}");
            else
                return CommandResponse.Text("No");
        }
    }
}
