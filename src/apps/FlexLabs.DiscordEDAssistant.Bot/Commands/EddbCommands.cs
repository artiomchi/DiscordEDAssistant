using Discord.Commands;
using FlexLabs.DiscordEDAssistant.Services.Integrations.Eddb;
using System;
using System.Diagnostics;

namespace FlexLabs.DiscordEDAssistant.Bot.Commands
{
    public static class EddbCommands
    {
        public static void CreateCommands_Eddb(this CommandService commandService)
        {
            commandService.CreateGroup("eddb", x =>
            {
                x.CreateCommand("sync")
                    .AddCheck(Bot.Check_IsServerAdmin)
                    .Do(async e =>
                    {
                        await e.Channel.SendIsTyping();

                        var sw = Stopwatch.StartNew();
                        using (var syncService = Bot.ServiceProvider.GetService(typeof(EddbSyncService)) as EddbSyncService)
                        {
                            await syncService.SyncAsync();
                        }

                        await e.Channel.SendMessage($"EDDB sync completed in `{sw.Elapsed}`.");
                    });

                x.CreateCommand("sync allsystems")
                    .AddCheck(Bot.Check_IsServerAdmin)
                    .Do(async e =>
                    {
                        await e.Channel.SendIsTyping();

                        var sw = Stopwatch.StartNew();
                        using (var syncService = Bot.ServiceProvider.GetService(typeof(EddbSyncService)) as EddbSyncService)
                        {
                            await syncService.SyncAllSystemsAsync();
                        }

                        await e.Channel.SendMessage($"EDDB sync completed in `{sw.Elapsed}`.");
                    });
            });

            commandService.CreateCommand("dist")
                .Description("Calculate distance between two systems")
                .Parameter("system1")
                .Parameter("system2")
                .Do(async e =>
                {
                    var system1 = e.GetArg("system1").Trim(',');
                    var system2 = e.GetArg("system2");

                    await e.Channel.SendIsTyping();

                    using (var dataService = Bot.ServiceProvider.GetService(typeof(EddbDataService)) as EddbDataService)
                    {
                        var sys1 = dataService.GetSystem(system1);
                        if (sys1 == null)
                        {
                            await e.Channel.SendMessage($"Unknown system: `{system1}`");
                            return;
                        }
                        var sys2 = dataService.GetSystem(system2);
                        if (sys2 == null)
                        {
                            await e.Channel.SendMessage($"Unknown system: `{system2}`");
                            return;
                        }

                        var dist = Math.Sqrt(Math.Pow(sys1.X - sys2.X, 2) + Math.Pow(sys1.Y - sys2.Y, 2) + Math.Pow(sys1.Z - sys2.Z, 2));
                        await e.Channel.SendMessage($"Distance between `{sys1.Name}` and `{sys2.Name}`: `{dist.ToString("N2")} ly`");
                    }
                });
        }
    }
}
