using System;

namespace FlexLabs.DiscordEDAssistant.Base.Extensions
{
    public static class VersionExtender
    {
        public static DateTime AsDateTime(this Version version)
            => new DateTime(2000, 1, 1).AddDays(version.Build).AddSeconds(version.Revision * 2);
    }
}
