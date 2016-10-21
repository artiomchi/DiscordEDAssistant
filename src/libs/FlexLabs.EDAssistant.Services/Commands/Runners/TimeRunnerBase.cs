using System;

namespace FlexLabs.EDAssistant.Services.Commands.Runners
{
    public abstract class TimeRunnerBase : IDisposable
    {
        public void Dispose() { }

        protected string FormatTime(DateTime time) => time.ToString("HH:mm:ss");

        protected TimeZoneInfo GetTimeZone(string timeZoneName)
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
