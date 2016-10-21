using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FlexLabs.EDAssistant.Services.Commands.Runners
{
    public class InaraWhoisRunner : IRunner
    {
        private readonly Models.Settings.InaraSettings _settings;
        public InaraWhoisRunner(IOptions<Models.Settings> settings)
        {
            _settings = settings.Value.Inara;
        }
        public void Dispose() { }

        public const string Prefix = "whois";
        public const string Template = "whois {cmdr}";
        string IRunner.Prefix => Prefix;
        string IRunner.Template => Template;
        public string Title => "Does a lookup for the CMDR info on Inara.cz";

        public async Task<CommandResponse> RunAsync(string[] arguments, object channelData)
        {
            var cmdrName = arguments[0];

            try
            {
                using (var client = new HttpClient())
                {
                    await client.PostAsync("http://inara.cz/login", new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        ["loginid"] = _settings.Login,
                        ["loginpass"] = _settings.Password,
                        ["formact"] = "ENT_LOGIN",
                        ["location"] = "intro",
                    }));

                    var searchResponse = await client.GetAsync("http://inara.cz/search?location=search&searchglobal=" + Uri.EscapeUriString(cmdrName));
                    if (!searchResponse.IsSuccessStatusCode)
                        return CommandResponse.Error("Failed to search for commander. Something wrong with Inara?");

                    var cmdrLink = Regex.Match(await searchResponse.Content.ReadAsStringAsync(), @"href=""/cmdr/(\d+)""");
                    if (!cmdrLink.Success)
                        return CommandResponse.Error("Commander not found");

                    var cmdrResponse = await client.GetAsync("http://inara.cz/cmdr/" + cmdrLink.Groups[1].Value);
                    if (!cmdrResponse.IsSuccessStatusCode)
                        return CommandResponse.Error("Failed load commanded details. Something wrong with Inara?");

                    var cmdrDoc = new HtmlDocument();
                    cmdrDoc.LoadHtml(await cmdrResponse.Content.ReadAsStringAsync());
                    var cmdrTableNode = cmdrDoc.DocumentNode.SelectSingleNode("//table[@class='pfl']");

                    cmdrName = cmdrTableNode.SelectSingleNode("//td[@class='header']").LastChild.InnerText.Trim();

                    var cells = new List<string[]>();
                    foreach (var cell in cmdrTableNode.SelectNodes("//td[span[@class='pflcellname']]"))
                    {
                        cells.Add(new[]
                        {
                            cell.FirstChild.InnerText.Trim(),
                            cell.LastChild.InnerText.Trim(),
                        });
                    }

                    var result = new CommandResponse();
                    result.Add($"CMDR `{cmdrName}`");
                    result.AddTable(cells.ToArray());
                    return result;
                }
            }
            catch
            {
                return CommandResponse.Error("General failure getting CMDR details");
            }
        }
    }
}
