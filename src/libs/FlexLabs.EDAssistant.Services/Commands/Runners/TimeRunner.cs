using System;

namespace FlexLabs.EDAssistant.Services.Commands.Runners
{
    public class TimeRunner
    {
        public static CommandResponse CurrentTime() => CommandResponse.Text($"Current in-game time: `{FormatTime(DateTime.UtcNow)}` (UTC)");

        public static CommandResponse Command_TimeIn(string timeZoneName, string timeStr)
        {
            var timeZone = GetTimeZone(timeZoneName);
            if (timeZone == null)
                return CommandResponse.Error("Could not understand the time zone name");

            DateTime time;
            var customTime = false;
            if (!string.IsNullOrWhiteSpace(timeStr))
            {
                customTime = true;
                if (!DateTime.TryParse(timeStr, out time))
                    return CommandResponse.Error("Could not understand the time");
            }
            else
            {
                time = DateTime.UtcNow;
            }

            var newTime = TimeZoneInfo.ConvertTimeFromUtc(time, timeZone);
            if (customTime)
                return CommandResponse.Text($"The time in `{timeZoneName}` at `{FormatTime(time)}` UTC will be `{FormatTime(newTime)}`");
            else
                return CommandResponse.Text($"The time in `{timeZoneName}` is `{FormatTime(newTime)}`");
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
