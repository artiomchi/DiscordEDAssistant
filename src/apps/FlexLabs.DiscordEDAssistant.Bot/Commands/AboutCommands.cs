using Discord.Commands;
using System;

namespace FlexLabs.DiscordEDAssistant.Bot.Commands
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
Current location : {Environment.MachineName}
Uptime           : {DateTime.UtcNow.Subtract(Bot.Started).ToString()}
Build            : {Program.GetVersion()}
Build time       : {Program.GetBuildTime()}
```";
                    if (Bot.ClientID != null)
                        message += $@"
If you want to add this bot to your server, follow this link:
https://discordapp.com/oauth2/authorize?client_id={Bot.ClientID}&scope=bot&permissions=3072";

                    return e.Channel.SendMessage(message);
                });
        }
    }
}
