using System;
using System.Threading.Tasks;

namespace FlexLabs.EDAssistant.Services.Commands.Runners
{
    public class TimeRunner : TimeRunnerBase, IRunner
    {
        public const string Prefix = "time";
        public const string Template = "time";
        string IRunner.Prefix => Prefix;
        string IRunner.Template => Template;
        public string Title => "Display the in-game time (UTC)";

        public Task<CommandResponse> RunAsync(string[] arguments, object channelData) => Task.FromResult(CommandResponse.Text($"Current in-game time: `{FormatTime(DateTime.UtcNow)}` (UTC)"));
    }
}
