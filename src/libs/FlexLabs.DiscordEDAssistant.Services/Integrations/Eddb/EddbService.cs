using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace FlexLabs.DiscordEDAssistant.Services.Integrations.Eddb
{
    public class EddbService
    {
        private async Task<string> DownloadFileAsync(string fileName)
        {
            using (var handler = new HttpClientHandler { AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate })
            using (var client = new HttpClient(handler))
            {
                var response = await client.GetAsync("https://eddb.io/archive/v4/" + fileName);
                if (!response.IsSuccessStatusCode)
                    return null;

                return await response.Content.ReadAsStringAsync();
            }
        }
        private async Task<TEntity[]> DownloadEntitiesAsync<TEntity>(string fileName)
        {
            var contents = await DownloadFileAsync(fileName);
            if (contents == null)
                return null;
            return JsonConvert.DeserializeObject<TEntity[]>(contents);
        }

        private Task<Models.Body[]> DownloadBodiesAsync() => DownloadEntitiesAsync<Models.Body>("bodies.json");
        private Task<Models.Commodity[]> DownloadCommoditiesAsync() => DownloadEntitiesAsync<Models.Commodity>("commmodities.json");
        private Task<Models.Module[]> DownloadModulesAsync() => DownloadEntitiesAsync<Models.Module>("modules.json");
        private Task<Models.Station[]> DownloadStationsAsync() => DownloadEntitiesAsync<Models.Station>("stations.json");
        private Task<Models.System[]> DownloadSystemsAsync() => DownloadEntitiesAsync<Models.System>("systems_populated.json");
        private Task<Models.System[]> DownloadSystemsAllAsync() => DownloadEntitiesAsync<Models.System>("systems.json");
    }
}
