using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace FlexLabs.EDAssistant.DiscordBot.Commands
{
    public static class TimeCommands
    {
        public static void CreateCommands_Time(this CommandService commandService)
        {
            commandService.CreateGroup("time", x =>
            {
                x.CreateCommand()
                    .Description("Display the in-game time (UTC)")
                    .Do(e => Command_Time(e.Channel));

                x.CreateCommand("in")
                    .Description("Convert in-game time to local time")
                    .Parameter("timezone", ParameterType.Required)
                    .Parameter("time", ParameterType.Optional)
                    .Do(e => Command_TimeIn(e.Channel, e.GetArg("timezone"), e.GetArg("time")));
            });
        }

        public static Task Command_Time(Channel channel)
            => channel.SendMessage($"Current in-game time: `{FormatTime(DateTime.UtcNow)}` (UTC)");

        public static async Task Command_TimeIn(Channel channel, string timeZoneName, string timeStr)
        {
            var timeZone = GetTimeZone(timeZoneName);
            if (timeZone == null)
            {
                await channel.SendMessage("Could not understand the time zone name");
                return;
            }

            DateTime time;
            var customTime = false;
            if (!string.IsNullOrWhiteSpace(timeStr))
            {
                customTime = true;
                if (!DateTime.TryParse(timeStr, out time))
                {
                    await channel.SendMessage("Could not understand the time");
                    return;
                }
            }
            else
            {
                time = DateTime.UtcNow;
            }

            var newTime = TimeZoneInfo.ConvertTimeFromUtc(time, timeZone);
            if (customTime)
                await channel.SendMessage($"The time in `{timeZoneName}` at `{FormatTime(time)}` UTC will be `{FormatTime(newTime)}`");
            else
                await channel.SendMessage($"The time in `{timeZoneName}` is `{FormatTime(newTime)}`");
        }

        private static String FormatTime(DateTime time) => time.ToString("HH:mm:ss");

        private static TimeZoneInfo GetTimeZone(String timeZoneName)
        {
            switch (timeZoneName.ToLowerInvariant())
            {
                case "pdt":
                case "pst": return TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
                case "cdt":
                case "cst": return TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
                case "edt":
                case "est": return TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                case "bst":
                case "gmt": return TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
                case "utc": return TimeZoneInfo.FindSystemTimeZoneById("UTC");
                case "cet": return TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time");
                case "eet": return TimeZoneInfo.FindSystemTimeZoneById("E. Europe Standard Time");
                case "ch": return TimeZoneInfo.FindSystemTimeZoneById("China Standard Time");
                case "aus": return TimeZoneInfo.FindSystemTimeZoneById("AUS Central Standard Time");
                case "eaus": return TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time");
                default:
                    try
                    {
                        return TimeZoneInfo.FindSystemTimeZoneById(timeZoneName);
                    }
                    catch
                    {
                        return null;
                    }
            }
        }
    }
}
