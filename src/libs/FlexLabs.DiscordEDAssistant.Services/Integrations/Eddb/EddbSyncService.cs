using FlexLabs.DiscordEDAssistant.Repositories.External.Eddb;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FlexLabs.DiscordEDAssistant.Services.Integrations.Eddb
{
    public class EddbSyncService : IDisposable
    {
        private readonly IEddbUpdateRepository _updateRepository;
        public EddbSyncService(IEddbUpdateRepository updateRepository)
        {
            _updateRepository = updateRepository;
        }

        public void Dispose() =>_updateRepository.Dispose();

        private async Task<bool> StreamProcessEntitiesAsync<TEntity>(string fileName, Func<IEnumerable<TEntity>, Task> action)
        {
            var queue = new BlockingCollection<TEntity>(5000);
            {
                var process = Task.Run(async () =>
                {
                    await action(queue.GetConsumingEnumerable());
                });

                try
                {
                    using (var handler = new HttpClientHandler { AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate })
                    using (var client = new HttpClient(handler) { Timeout = TimeSpan.FromHours(6) })
                    {
                        var response = await client.GetAsync("https://eddb.io/archive/v4/" + fileName, HttpCompletionOption.ResponseHeadersRead);
                        if (!response.IsSuccessStatusCode)
                            return false;

                        var serializer = new JsonSerializer();
                        using (var stream = await response.Content.ReadAsStreamAsync())
                        using (var tReader = new StreamReader(stream))
                        using (var reader = new JsonTextReader(tReader))
                        {
                            if (!reader.Read() || reader.TokenType != JsonToken.StartArray)
                                return false;//invalid file?

                            while (reader.Read() && reader.TokenType == JsonToken.StartObject)
                            {
                                queue.Add(serializer.Deserialize<TEntity>(reader));
                            }
                        }
                    }
                }
                finally
                {
                    queue.CompleteAdding();
                    await process;
                }
            }
            return true;
        }

        public async Task<bool> SyncAsync()
        {
            _updateRepository.ClearAll();

            await StreamProcessEntitiesAsync<Models.Commodity>("commodities.json",
                systems => _updateRepository.BulkUploadAsync(systems.Select(s => s.Translate())));

            await StreamProcessEntitiesAsync<Models.Module>("modules.json",
                systems => _updateRepository.BulkUploadAsync(systems.Select(s => s.Translate())));

            await StreamProcessEntitiesAsync<Models.StarSystem>("systems_populated.json",
                systems => _updateRepository.BulkUploadAsync(systems.Select(s => s.Translate())));

            await StreamProcessEntitiesAsync<Models.Station>("stations.json",
                async stations =>
                {
                    using (var stationsQueue = new BlockingCollection<DiscordEDAssistant.Models.External.Eddb.Station>())
                    using (var stationModulesQueue = new BlockingCollection<Tuple<int, int>>(40000))
                    using (var stationShipsQueue = new BlockingCollection<Tuple<int, string>>(40000))
                    {
                        var stationsTask = Task.Run(() => _updateRepository.BulkUploadAsync(stationsQueue.GetConsumingEnumerable()));
                        var modulesTask = Task.Run(() => _updateRepository.BulkUploadStationModulesAsync(stationModulesQueue.GetConsumingEnumerable()));
                        var shipsTask = Task.Run(() => _updateRepository.BulkUploadStationShipsAsync(stationShipsQueue.GetConsumingEnumerable()));

                        try
                        {
                            foreach (var station in stations.Select(s => s.Translate()))
                            {
                                stationsQueue.Add(station);
                                if (station.SellingModules?.Length > 0)
                                    foreach (var moduleID in station.SellingModules)
                                        stationModulesQueue.Add(new Tuple<int, int>(station.ID, moduleID));
                                if (station.SellingShips?.Length > 0)
                                    foreach (var ship in station.SellingShips)
                                        stationShipsQueue.Add(new Tuple<int, string>(station.ID, ship));
                            }
                        }
                        finally
                        {
                            stationsQueue.CompleteAdding();
                            stationModulesQueue.CompleteAdding();
                            stationShipsQueue.CompleteAdding();

                            await stationsTask;
                            await modulesTask;
                            await shipsTask;
                        }
                    }
                });

            _updateRepository.MergeAll();

            return true;
        }

        public async Task SyncAllSystemsAsync()
        {
            _updateRepository.ClearAll();

            await StreamProcessEntitiesAsync<Models.StarSystem>("systems.json", systems => _updateRepository.BulkUploadAsync(systems.Select(s => s.Translate())));

            _updateRepository.MergeAllSystems();
        }
    }
}
