﻿using System;
using System.Threading.Tasks;

namespace FlexLabs.EDAssistant.Services.Commands.Runners
{
    public class TimeInRunner : TimeRunnerBase, IRunner
    {
        public string Prefix => "time in";
        public string Template => "time in {timezone} {time}";
        public string Title => "Convert in-game time to local time";

        public Task<CommandResponse> RunAsync(string[] arguments)
        {
            if (arguments.Length < 2)
                return Task.FromResult(CommandResponse.Nop);

            return Task.FromResult(Run(arguments[0], arguments[1]));
        }

        private CommandResponse Run(string timeZoneName, string timeStr)
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
    }
}
