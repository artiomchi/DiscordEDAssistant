using System;

namespace FlexLabs.EDAssistant.Base.Extensions
{
    public static class VersionExtender
    {
        public static DateTime AsDateTime(this Version version)
            => new DateTime(2000, 1, 1).AddDays(version.Build).AddSeconds(version.Revision * 2);
    }
}
