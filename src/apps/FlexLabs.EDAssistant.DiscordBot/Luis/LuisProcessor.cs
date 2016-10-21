using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Discord;

namespace FlexLabs.EDAssistant.DiscordBot.Luis
{
    public static class LuisProcessor
    {
        private static string AppID => Models.Settings.Instance.Luis.AppId;
        private static string SubKey => Models.Settings.Instance.Luis.SubscriptionKey;

        public static async Task Process(Channel channel, string message)
        {
            var uri = $"https://api.projectoxford.ai/luis/v1/application?id={AppID}&subscription-key={SubKey}&q=" + Uri.EscapeUriString(message);
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<LuisResponse>(content);

                if (data.intents.Length == 0)
                    return;

                //switch (data.intents[0].intent)
                //{
                //    case "Distance":
                //        var systems = data.entities.Where(en => en.type == "System").ToList();
                //        if (systems.Count > 1)
                //        {
                //            await Commands.EddbCommands.Command_Dist(channel, systems[0].entity, systems[1].entity);
                //            return;
                //        }
                //        break;

                //    case "TimeCurrent":
                //        await Commands.TimeCommands.Command_Time(channel);
                //        return;

                //    case "TimeIn":
                //        var timeZones = data.entities.Where(en => en.type == "TimeZone").ToList();
                //        if (timeZones.Count > 0)
                //        {
                //            await Commands.TimeCommands.Command_TimeIn(channel, timeZones[0].entity, null);
                //            return;
                //        }
                //        break;

                //    case "TimeInAt":
                //        var timeZones2 = data.entities.Where(en => en.type == "TimeZone").ToList();
                //        var times = data.entities.Where(en => en.type == "builtin.datetime.time").ToList();
                //        if (timeZones2.Count > 0 && times.Count > 0)
                //        {
                //            await Commands.TimeCommands.Command_TimeIn(channel, timeZones2[0].entity, times[0].entity);
                //            return;
                //        }
                //        break;

                //    case "ModulesNear":
                //        var systems2 = data.entities.Where(en => en.type == "System").ToList();
                //        var modules = data.entities.Where(en => en.type == "Module").ToList();
                //        if (systems2.Count > 0 && modules.Count > 0)
                //        {
                //            await Commands.EddbCommands.Commands_ModulesNear(channel, systems2[0].entity, modules.Select(e => e.entity).ToArray());
                //            return;
                //        }
                //        break;
                //}
                await channel.SendMessage("Sorry, I didn't understand you");
            }
        }
    }
}
