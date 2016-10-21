using FlexLabs.EDAssistant.Services.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FlexLabs.EDAssistant.DiscordBot.Runners
{
    public class AboutRunner : IRunner
    {
        public void Dispose() { }

        public string Prefix => "about";
        public string Template => "about";
        public string Title => "Display bot information";

        public Task<CommandResponse> RunAsync(string[] arguments, object channelData)
        {
            var message =
        $@"```http
Status           : OK
Servers Joined   : {Bot.Client.Servers.Count()}
Uptime           : {DateTime.UtcNow.Subtract(Bot.Started).ToString()}
Build            : {Program.GetVersion()}
Build Time       : {Program.GetBuildTime()}
```";
                    if (Models.Settings.Instance.Discord.ClientID != null)
                        message += $@"
If you want to add this bot to your server, follow this link:
https://discordapp.com/oauth2/authorize?client_id={Models.Settings.Instance.Discord.ClientID}&scope=bot&permissions=3072";

            return Task.FromResult(CommandResponse.Text(message));
        }
    }
}
