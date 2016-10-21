using FlexLabs.EDAssistant.Services.Integrations.Eddb;
using System;
using System.Threading.Tasks;

namespace FlexLabs.EDAssistant.Services.Commands.Runners
{
    public class EddbSystemDistanceRunner : IRunner
    {
        private readonly EddbDataService _dataService;
        public EddbSystemDistanceRunner(EddbDataService dataService)
        {
            _dataService = dataService;
        }
        public void Dispose() => _dataService.Dispose();

        public string Prefix => "dist";
        public string Template => "dist {system} {system}";
        public string Title => "Calculate distance between two systems";

        public Task<CommandResponse> RunAsync(string[] arguments, object channelData)
        {
            if (arguments.Length < 2)
                return Task.FromResult(CommandResponse.Nop);

            return Task.FromResult(Run(arguments[0], arguments[1]));
        }

        private CommandResponse Run(string system1, string system2)
        {
            var sys1 = _dataService.GetSystem(system1);
            if (sys1 == null)
                return CommandResponse.Error($"Unknown system: `{system1}`");

            var sys2 = _dataService.GetSystem(system2);
            if (sys2 == null)
            return CommandResponse.Error($"Unknown system: `{system2}`");

            var dist = Math.Sqrt(Math.Pow(sys1.X - sys2.X, 2) + Math.Pow(sys1.Y - sys2.Y, 2) + Math.Pow(sys1.Z - sys2.Z, 2));
            return CommandResponse.Text($"Distance between `{sys1.Name}` and `{sys2.Name}` is: `{dist.ToString("N2")} ly`");
        }
    }
}
