using Discord;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FlexLabs.EDAssistant.DiscordBot.Runners
{
    public static class WelcomeRunnerHelper
    {
        public static string ProcessMessageChannelLinks(Server server, User user, string message)
        {
            var channelMentions = Regex.Matches(message, @"#(\w+)\b");
            foreach (var match in channelMentions.OfType<Match>().Select(m => m.Groups[1].Value).Distinct())
            {
                var channel = server.FindChannels(match).FirstOrDefault();
                if (channel != null)
                    message = Regex.Replace(message, $@"#{match}\b", channel.Mention);
            }
            message = message.IndexOf("{user}") >= 0
                ? message.Replace("{user}", user.Mention)
                : user.Mention + " " + message;
            return message;
        }
    }
}
