using Discord.Commands;
using System;
using System.Linq;

namespace FlexLabs.EDAssistant.DiscordBot.Commands
{
    public static class AboutCommands
    {
        public static void CreateCommands_About(this CommandService commandService)
        {
            commandService.CreateCommand("about")
                .Alias("uptime", "status")
                .Description("Display server information")
                .Do(e =>
                {
                    var message =
        $@"```http
Status           : OK
Servers Joined   : {e.Channel.Client.Servers.Count()}
Uptime           : {DateTime.UtcNow.Subtract(Bot.Started).ToString()}
Build            : {Program.GetVersion()}
Build Time       : {Program.GetBuildTime()}
```";
                    if (Models.Settings.Instance.Discord.ClientID != null)
                        message += $@"
If you want to add this bot to your server, follow this link:
https://discordapp.com/oauth2/authorize?client_id={Models.Settings.Instance.Discord.ClientID}&scope=bot&permissions=3072";

                    return e.Channel.SendMessage(message);
                });
        }
    }
}
