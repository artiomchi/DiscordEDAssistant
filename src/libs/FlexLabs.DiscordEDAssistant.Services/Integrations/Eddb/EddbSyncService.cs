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

        private async Task<TEntity[]> DownloadEntitiesAsync<TEntity>(string fileName)
        {
            using (var handler = new HttpClientHandler { AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate })
            using (var client = new HttpClient(handler))
            {
                var response = await client.GetAsync("https://eddb.io/archive/v4/" + fileName);
                if (!response.IsSuccessStatusCode)
                    return null;

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TEntity[]>(json);
            }
        }

        private async Task StreamProcessEntitiesAsync<TEntity>(string fileName, Func<List<TEntity>, Task> action)
        {
            using (var queue = new BlockingCollection<List<TEntity>>(5))
            {
                var process = Task.Run(async () =>
                {
                    foreach (var list in queue.GetConsumingEnumerable())
                        await action(list);
                });

                using (var handler = new HttpClientHandler { AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate })
                using (var client = new HttpClient(handler))
                {
                    var response = await client.GetAsync("https://eddb.io/archive/v4/" + fileName, HttpCompletionOption.ResponseHeadersRead);
                    if (!response.IsSuccessStatusCode)
                        return;

                    using (var stream = await response.Content.ReadAsStreamAsync())
                    using (var reader = new StreamReader(stream))
                    {
                        string line;
                        var buffer = new List<TEntity>();
                        while ((line = await reader.ReadLineAsync()) != null)
                        {
                            buffer.Add(JsonConvert.DeserializeObject<TEntity>(line));
                            if (buffer.Count > 10000)
                            {
                                queue.Add(buffer);
                                buffer = new List<TEntity>();
                            }
                        }
                        if (buffer.Count > 0)
                            queue.Add(buffer);
                        queue.CompleteAdding();
                    }
                }

                await process;
            }
        }

        private Task<Models.Body[]> DownloadBodiesAsync() => DownloadEntitiesAsync<Models.Body>("bodies.json");
        private Task<Models.Commodity[]> DownloadCommoditiesAsync() => DownloadEntitiesAsync<Models.Commodity>("commmodities.json");
        private Task<Models.Module[]> DownloadModulesAsync() => DownloadEntitiesAsync<Models.Module>("modules.json");
        private Task<Models.Station[]> DownloadStationsAsync() => DownloadEntitiesAsync<Models.Station>("stations.json");
        private Task<Models.StarSystem[]> DownloadSystemsAsync() => DownloadEntitiesAsync<Models.StarSystem>("systems_populated.json");
        private Task<Models.StarSystem[]> DownloadSystemsAllAsync() => DownloadEntitiesAsync<Models.StarSystem>("systems.json");

        public async Task SyncAsync()
        {
            _updateRepository.ClearAll();

            await StreamProcessEntitiesAsync<Models.Module>("modules.jsonl",
                modules => _updateRepository.BulkUploadAsync(modules.Select(m => m.Translate())));

            await StreamProcessEntitiesAsync<Models.StarSystem>("systems_populated.jsonl",
                systems => _updateRepository.BulkUploadAsync(systems.Select(s => s.Translate())));

            _updateRepository.MergeAll();
        }

        public async Task SyncAllSystemsAsync()
        {
            _updateRepository.ClearAll();

            await StreamProcessEntitiesAsync<Models.StarSystem>("systems.jsonl", systems => _updateRepository.BulkUploadAsync(systems.Select(s => s.Translate())));

            _updateRepository.MergeAllSystems();
        }
    }
}
