using Discord.Commands;
using HtmlAgilityPack;
using RestSharp.Extensions.MonoHttp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace FlexLabs.EDAssistant.DiscordBot.Commands
{
    public static class InaraCommands
    {
        private static String InaraLogin => Models.Settings.Instance.Inara.Login;
        private static String InaraPass => Models.Settings.Instance.Inara.Password;

        public static void CreateCommands_Inara(this CommandService commandService)
        {
            if (InaraLogin == null || InaraPass == null)
                return;

            commandService.CreateCommand("whois")
                .Description("Does a lookup for the CMDR info on Inara.cz")
                .Parameter("cmdr", ParameterType.Unparsed)
                .Do(Command_Whois);
        }

        private static async Task Command_Whois(CommandEventArgs e)
        {
            var cmdrName = e.GetArg("cmdr");

            try
            {
                using (var timer = new Timer(delegate { e.Channel.SendIsTyping(); }, null, 0, 3000))
                using (var client = new HttpClient())
                {
                    await client.PostAsync("http://inara.cz/login", new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        ["loginid"] = InaraLogin,
                        ["loginpass"] = InaraPass,
                        ["formact"] = "ENT_LOGIN",
                        ["location"] = "intro",
                    }));

                    var searchResponse = await client.GetAsync("http://inara.cz/search?location=search&searchglobal=" + HttpUtility.UrlEncode(cmdrName));
                    if (!searchResponse.IsSuccessStatusCode)
                    {
                        await e.Channel.SendMessage("Failed to search for commander. Something wrong with Inara?");
                        return;
                    }

                    var cmdrLink = Regex.Match(await searchResponse.Content.ReadAsStringAsync(), @"href=""/cmdr/(\d+)""");
                    if (!cmdrLink.Success)
                    {
                        await e.Channel.SendMessage("Commander not found");
                        return;
                    }

                    var cmdrResponse = await client.GetAsync("http://inara.cz/cmdr/" + cmdrLink.Groups[1].Value);
                    if (!cmdrResponse.IsSuccessStatusCode)
                    {
                        await e.Channel.SendMessage("Failed load commanded details. Something wrong with Inara?");
                        return;
                    }

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

                    await e.Channel.SendMessage($@"CMDR `{cmdrName}`:
```
{Helpers.FormatAsTable(cells.ToArray())}
```");
                }
            }
            catch
            {
                await e.Channel.SendMessage("General failure getting CMDR details");
            }
        }
    }
}
