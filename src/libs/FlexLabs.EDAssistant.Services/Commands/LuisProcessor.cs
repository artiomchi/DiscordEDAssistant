using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FlexLabs.EDAssistant.Services.Commands
{
    public class LuisProcessor
    {
        private static string AppID => Models.Settings.Instance.Luis.AppId;
        private static string SubKey => Models.Settings.Instance.Luis.SubscriptionKey;

        public async Task<string> Process(string message)
        {
            var uri = $"https://api.projectoxford.ai/luis/v1/application?id={AppID}&subscription-key={SubKey}&q=" + Uri.EscapeUriString(message);
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<LuisResponse>(content);

                if (data.intents.Length == 0)
                    return null;

                switch (data.intents[0].intent)
                {
                    case "Distance":
                        var systems = data.entities.Where(en => en.type == "System").ToList();
                        if (systems.Count > 1)
                            return $"{Runners.EddbSystemDistanceRunner.Prefix} {systems[0].entity}, {systems[1].entity}";
                        break;

                    case "TimeCurrent":
                        return $"{Runners.TimeRunner.Prefix}";

                    case "TimeIn":
                        var timeZones = data.entities.Where(en => en.type == "TimeZone").ToList();
                        if (timeZones.Count > 0)
                            return $"{Runners.TimeInRunner.Prefix} {timeZones[0].entity}";
                        break;

                    case "TimeInAt":
                        var timeZones2 = data.entities.Where(en => en.type == "TimeZone").ToList();
                        var times = data.entities.Where(en => en.type == "builtin.datetime.time").ToList();
                        if (timeZones2.Count > 0 && times.Count > 0)
                            return $"{Runners.TimeInRunner.Prefix} {timeZones2[0].entity}, {times[0].entity}";
                        break;

                    case "ModulesNear":
                        var systems2 = data.entities.Where(en => en.type == "System").ToList();
                        var modules = data.entities.Where(en => en.type == "Module").ToList();
                        if (systems2.Count > 0 && modules.Count > 0)
                            return $"{Runners.EddbModuleSearchRunner.Prefix} {systems2[0].entity}, {String.Join(", ", modules.Select(e => e.entity))}";
                        break;
                }
                return null;
            }
        }

        public class LuisResponse
        {
            public string query { get; set; }
            public Intent[] intents { get; set; }
            public Entity[] entities { get; set; }
        }

        public class Intent
        {
            public string intent { get; set; }
            public float score { get; set; }
            public Action[] actions { get; set; }
        }

        public class Action
        {
            public bool triggered { get; set; }
            public string name { get; set; }
            public Parameter[] parameters { get; set; }
        }

        public class Parameter
        {
            public string name { get; set; }
            public bool required { get; set; }
            public Value[] value { get; set; }
        }

        public class Value
        {
            public string entity { get; set; }
            public string type { get; set; }
            public float score { get; set; }
        }

        public class Entity
        {
            public string entity { get; set; }
            public string type { get; set; }
            public int startIndex { get; set; }
            public int endIndex { get; set; }
            public float score { get; set; }
        }
    }
}
