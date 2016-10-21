using System;
using System.Threading.Tasks;

namespace FlexLabs.EDAssistant.Services.Commands.Runners
{
    public class TimeRunner : TimeRunnerBase, IRunner
    {
        public string Prefix => "time";
        public string Template => "time";
        public string Title => "Display the in-game time (UTC)";
        public Task<CommandResponse> RunAsync(string[] arguments, object channelData) => Task.FromResult(CommandResponse.Text($"Current in-game time: `{FormatTime(DateTime.UtcNow)}` (UTC)"));
    }
}
