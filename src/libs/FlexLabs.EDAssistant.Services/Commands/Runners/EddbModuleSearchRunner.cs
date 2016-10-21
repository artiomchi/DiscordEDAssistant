using FlexLabs.EDAssistant.Base.Extensions;
using FlexLabs.EDAssistant.Services.Integrations.Eddb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlexLabs.EDAssistant.Services.Commands.Runners
{
    public class EddbModuleSearchRunner : IRunner
    {
        private readonly EddbDataService _dataService;
        public EddbModuleSearchRunner(EddbDataService dataService)
        {
            _dataService = dataService;
        }
        public void Dispose() => _dataService.Dispose();

        public string Prefix => "modules near";
        public string Template => "modules near {system} {modules}";
        public string Title => "Find modules closest to the current system";

        public Task<CommandResponse> RunAsync(string[] arguments)
        {
            if (arguments.Length < 0)
                return Task.FromResult(CommandResponse.Nop);

            return RunAsync(arguments[0], arguments.Skip(1).ToArray());
        }

        private async Task<CommandResponse> RunAsync(string systemName, string[] modules)
        {
            var starSystem = _dataService.GetSystem(systemName);
            if (starSystem == null)
                return CommandResponse.Error($"Unknown system: `{systemName}`");

            var moduleIDs = new List<int>();
            foreach (var arg in modules)
            {
                var moduleID = _dataService.FindModuleID(arg);
                if (moduleID == null)
                    return CommandResponse.Error($"Unknown module: `{arg}`");
                moduleIDs.Add(moduleID.Value);
            }

            var stations = (await _dataService.FindClosestStationsWithModulesAsync(starSystem, moduleIDs)).ToList();
            if (stations.Count == 0)
                return CommandResponse.Error("Nobosy seems to have it");

            var headings = new[] { new[] { "System", "Distance", "Station", "Pad", "Plan", "Dst to star", "Updated" } };
            var data = stations.Select(s => new[]
            {
                s.SystemName,
                s.DistanceToSystem.ToString("N2") + " ly",
                s.Name,
                s.MaxLandingPadSize?.ToString(),
                s.IsPlanetary ? "*" : "",
                s.DistanceToStar.HasValue ? s.DistanceToStar.Value.ToString("N0") + " ls" : "?",
                s.MarketUpdatedAt.HasValue ? DateTime.UtcNow.Subtract(s.MarketUpdatedAt.Value).ToFriendlyString() : "-",
            });
            var rightAligned = new[] { 1, 5, 6 };

            var result = new CommandResponse();
            result.Add($@"Closest stations to `{starSystem.Name}` with that sell `{String.Join("`, `", modules)}`:");
            result.AddTable(headings.Concat(data).ToArray(), rightAligned);
            return result;
        }
    }
}
