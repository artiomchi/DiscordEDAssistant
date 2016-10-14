using Discord.Commands;
using FlexLabs.DiscordEDAssistant.Services.Integrations.Eddb;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using FlexLabs.DiscordEDAssistant.Base.Extensions;
using FlexLabs.DiscordEDAssistant.Bot.Extensions;
using Discord;

namespace FlexLabs.DiscordEDAssistant.Bot.Commands
{
    public static class EddbCommands
    {
        public static void CreateCommands_Eddb(this CommandService commandService)
        {
            commandService.CreateCommand("dist")
                .Description("Calculate distance between two systems")
                .Parameter("system1")
                .Parameter("system2")
                .Do(e => Command_Dist(e.Channel, e.GetArg("system1").Trim(','), e.GetArg("system2")));

            commandService.CreateCommand("modules near")
                .Description("Find modules closest to the current system")
                .Parameter("system")
                .Parameter("module", ParameterType.Multiple)
                .Do(e => Commands_ModulesNear(e.Channel, e.GetArg("system"), e.Args.Skip(1).ToArray()));

            commandService.CreateGroup("eddb", x =>
            {
                x.CreateCommand("sync")
                    .Hide()
                    .ModCommand()
                    .Description("Sync the core data from EDDB")
                    .AddCheck(Bot.Check_IsServerAdmin)
                    .Do(Command_Eddb_Sync);

                x.CreateCommand("sync allsystems")
                    .Hide()
                    .ModCommand()
                    .Description("Sync the full system list from EDDB")
                    .AddCheck(Bot.Check_IsServerAdmin)
                    .Do(Command_Eddb_Sync_AllSystems);
            });
        }

        public static async Task Command_Dist(Channel channel, string system1, string system2)
        {
            using (var timer = new Timer(delegate {channel.SendIsTyping(); }, null, 0, 3000))
            using (var dataService = Bot.ServiceProvider.GetService<EddbDataService>())
            {
                var sys1 = dataService.GetSystem(system1);
                if (sys1 == null)
                {
                    await channel.SendMessage($"Unknown system: `{system1}`");
                    return;
                }
                var sys2 = dataService.GetSystem(system2);
                if (sys2 == null)
                {
                    await channel.SendMessage($"Unknown system: `{system2}`");
                    return;
                }

                var dist = Math.Sqrt(Math.Pow(sys1.X - sys2.X, 2) + Math.Pow(sys1.Y - sys2.Y, 2) + Math.Pow(sys1.Z - sys2.Z, 2));
                await channel.SendMessage($"Distance between `{sys1.Name}` and `{sys2.Name}` is: `{dist.ToString("N2")} ly`");
            }
        }

        public static async Task Commands_ModulesNear(Channel channel, string systemName, string[] modules)
        {
            using (var timer = new Timer(delegate { channel.SendIsTyping(); }, null, 0, 3000))
            using (var dataService = Bot.ServiceProvider.GetService<EddbDataService>())
            {
                var starSystem = dataService.GetSystem(systemName);
                if (starSystem == null)
                {
                    await channel.SendMessage($"Unknown system: `{systemName}`");
                    return;
                }

                var moduleIDs = new List<int>();
                foreach (var arg in modules)
                {
                    var moduleID = dataService.FindModuleID(arg);
                    if (moduleID == null)
                    {
                        await channel.SendMessage($"Unknown module: `{arg}`");
                        return;
                    }
                    moduleIDs.Add(moduleID.Value);
                }

                var stations = (await dataService.FindClosestStationsWithModulesAsync(starSystem, moduleIDs)).ToList();
                if (stations.Count == 0)
                {
                    await channel.SendMessage("Nobosy seems to have it");
                    return;
                }

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
                await channel.SendMessage($@"Closest stations to `{starSystem.Name}` with that sell `{String.Join("`, `", modules)}`:
```
{Helpers.FormatAsTable(headings.Concat(data).ToArray(), rightAligned)}
```");
            }
        }

        private static async Task Command_Eddb_Sync(CommandEventArgs e)
        {
            var sw = Stopwatch.StartNew();
            using (var timer = new Timer(delegate { e.Channel.SendIsTyping(); }, null, 0, 3000))
            using (var syncService = Bot.ServiceProvider.GetService<EddbSyncService>())
            {
                await syncService.SyncAsync();
            }

            await e.Channel.SendMessage($"EDDB sync completed in `{sw.Elapsed}`.");
        }

        private static async Task Command_Eddb_Sync_AllSystems(CommandEventArgs e)
        {
            var sw = Stopwatch.StartNew();
            using (var timer = new Timer(delegate { e.Channel.SendIsTyping(); }, null, 0, 3000))
            using (var syncService = Bot.ServiceProvider.GetService<EddbSyncService>())
            {
                await syncService.SyncAllSystemsAsync();
            }

            await e.Channel.SendMessage($"EDDB sync completed in `{sw.Elapsed}`.");
        }
    }
}
