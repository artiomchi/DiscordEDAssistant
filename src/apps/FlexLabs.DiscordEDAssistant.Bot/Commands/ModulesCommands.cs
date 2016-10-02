using Discord.Commands;
using FlexLabs.DiscordEDAssistant.Services.Integrations.Eddb;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FlexLabs.DiscordEDAssistant.Bot.Commands
{
    public static class ModulesCommands
    {
        public static void CreateCommands_Modules(this CommandService commandService)
        {
            commandService.CreateCommand("modules near")
                .Description("Find modules closest to the current system")
                .Parameter("system")
                .Parameter("module", ParameterType.Multiple)
                .Do(Commands_ModulesNear);
        }

        private static async Task Commands_ModulesNear(CommandEventArgs e)
        {
            var systemName = e.GetArg("system");
            var moduleName = e.GetArg("module");

            using (var timer = new Timer(delegate { e.Channel.SendIsTyping(); }, null, 0, 3000))
            using (var dataService = Bot.ServiceProvider.GetService<EddbDataService>())
            {
                var starSystem = dataService.GetSystem(systemName);
                if (starSystem == null)
                {
                    await e.Channel.SendMessage($"Unknown system: `{systemName}`");
                    return;
                }

                var moduleID = dataService.FindModuleID(moduleName);
                if (moduleID == null)
                {
                    await e.Channel.SendMessage($"Unknown module: `{moduleName}`");
                    return;
                }

                var stations = dataService.FindClosestStationsWithModules(starSystem, new[] { moduleID.Value }).Take(10).ToList();
                if (stations.Count == 0)
                {
                    await e.Channel.SendMessage("Nobosy seems to have it");
                    return;
                }

                var headings = new[] { new[] { "System", "Distance", "Station", "Dst to star" } };
                var data = stations.Select(s => new[] { s.SystemName, s.DistanceToSystem.ToString("N2") + " ly", s.Name, s.DistanceToStar.Value.ToString("N") + " ls" });
                await e.Channel.SendMessage($@"Closest stations with the desired modules:
```
{Helpers.FormatAsTable(headings.Concat(data).ToArray())}
```");
            }
        }
    }
}
