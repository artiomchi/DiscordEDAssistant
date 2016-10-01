using FlexLabs.DiscordEDAssistant.Repositories.External.Eddb;
using Newtonsoft.Json;
using System;
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

        private async Task StreamProcessEntitiesAsync<TEntity>(string fileName, Action<List<TEntity>> action)
        {
            using (var handler = new HttpClientHandler { AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate })
            using (var client = new HttpClient(handler))
            {
                var response = await client.GetAsync("https://eddb.io/archive/v4/" + fileName, HttpCompletionOption.ResponseHeadersRead);
                if (!response.IsSuccessStatusCode)
                    return;

                using (var stream = await response.Content.ReadAsStreamAsync())
                using (var reader = new StreamReader(stream))
                {
                    var buffer = new List<TEntity>();
                    int i = 0;
                    string line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        buffer.Add(JsonConvert.DeserializeObject<TEntity>(line));
                        if (i++ > 50000)
                        {
                            action(buffer);
                            buffer = new List<TEntity>();
                            i = 0;
                        }
                    }
                    if (buffer.Count > 0)
                        action(buffer);
                }
            }
        }

        private Task<Models.Body[]> DownloadBodiesAsync() => DownloadEntitiesAsync<Models.Body>("bodies.json");
        private Task<Models.Commodity[]> DownloadCommoditiesAsync() => DownloadEntitiesAsync<Models.Commodity>("commmodities.json");
        private Task<Models.Module[]> DownloadModulesAsync() => DownloadEntitiesAsync<Models.Module>("modules.json");
        private Task<Models.Station[]> DownloadStationsAsync() => DownloadEntitiesAsync<Models.Station>("stations.json");
        private Task<Models.System[]> DownloadSystemsAsync() => DownloadEntitiesAsync<Models.System>("systems_populated.json");
        private Task<Models.System[]> DownloadSystemsAllAsync() => DownloadEntitiesAsync<Models.System>("systems.json");

        public async Task SyncAsync()
        {
            _updateRepository.ClearAll();

            var modules = await DownloadModulesAsync();
            _updateRepository.UploadAll(modules.Select(m => m.Translate()));

            await StreamProcessEntitiesAsync<Models.System>("systems_populated.jsonl", systems => _updateRepository.UploadAll(systems.Select(s => s.Translate())));

            _updateRepository.MergeAll();
        }

        public async Task SyncAllSystemsAsync()
        {
            _updateRepository.ClearAll();

            await StreamProcessEntitiesAsync<Models.System>("systems.jsonl", systems => _updateRepository.BulkUploadSystemsAsync(systems.Select(s => s.Translate())));

            _updateRepository.MergeAllSystems();
        }
    }
}
