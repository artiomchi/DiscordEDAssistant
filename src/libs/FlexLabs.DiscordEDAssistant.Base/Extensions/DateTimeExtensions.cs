using System;

namespace FlexLabs.DiscordEDAssistant.Base.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime UnixTimeStampToDateTime(this int timestamp)
            => new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(timestamp);

        public static string ToFriendlyString(this TimeSpan span)
        {
            if (span.TotalDays > 1)
                return span.TotalDays.ToString("N0") + " days";
            else if (span.TotalHours > 1)
                return span.TotalHours.ToString("N0") + " hours";
            else if (span.TotalMinutes > 1)
                return span.TotalMinutes.ToString("N0") + " hours";
            else
                return span.TotalSeconds.ToString("N0") + " hours";
        }
    }
}
