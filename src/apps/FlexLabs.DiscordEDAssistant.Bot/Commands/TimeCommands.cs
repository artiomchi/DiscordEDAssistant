using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace FlexLabs.DiscordEDAssistant.Bot.Commands
{
    public static class TimeCommands
    {
        public static void CreateCommands_Time(this CommandService commandService)
        {
            commandService.CreateGroup("time", x =>
            {
                x.CreateCommand()
                    .Description("Display the in-game time (UTC)")
                    .Do(Command_Time);

                x.CreateCommand("in")
                    .Description("Convert in-game time to local time")
                    .Parameter("timezone", ParameterType.Required)
                    .Parameter("time", ParameterType.Optional)
                    .Do(Command_TimeIn);
            });
        }

        private static Task Command_Time(CommandEventArgs e)
            => e.Channel.SendMessage($"Current in-game time: `{FormatTime(DateTime.UtcNow)}` (UTC)");

        private static async Task Command_TimeIn(CommandEventArgs e)
        {
            var timeZoneName = e.GetArg("timezone");
            var timeZone = GetTimeZone(timeZoneName);
            if (timeZone == null)
            {
                await e.Channel.SendMessage("Could not understand the time zone name");
                return;
            }

            DateTime time;
            var customTime = false;
            if (!string.IsNullOrWhiteSpace(e.GetArg("time")))
            {
                customTime = true;
                if (!DateTime.TryParse(e.GetArg("time"), out time))
                {
                    await e.Channel.SendMessage("Could not understand the time");
                    return;
                }
            }
            else
            {
                time = DateTime.UtcNow;
            }

            var newTime = TimeZoneInfo.ConvertTimeFromUtc(time, timeZone);
            if (customTime)
                await e.Channel.SendMessage($"The time in `{timeZoneName}` at `{FormatTime(time)}` UTC will be `{FormatTime(newTime)}`");
            else
                await e.Channel.SendMessage($"The time in `{timeZoneName}` is `{FormatTime(newTime)}`");
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
