using System;

namespace FlexLabs.DiscordEDAssistant.Base.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime UnixTimeStampToDateTime(this int timestamp)
            => new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(timestamp);
    }
}
