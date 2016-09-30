using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FlexLabs.DiscordEDAssistant.Services.Integrations
{
    public class EddbService
    {
        private async Task<Byte[]> DownloadFileAsync(string fileName)
        {
            using (var handler = new HttpClientHandler { AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate })
            using (var client = new HttpClient(handler))
            {
                var response = await client.GetAsync("https://eddb.io/archive/v4/" + fileName);
                if (!response.IsSuccessStatusCode)
                    return null;

                var data = await response.Content.ReadAsByteArrayAsync();
                return data;
            }
        }
    }
}
