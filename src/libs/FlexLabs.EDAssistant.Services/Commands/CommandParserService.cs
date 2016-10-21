using System;
using System.Linq;
using System.Threading.Tasks;

namespace FlexLabs.EDAssistant.Services.Commands
{
    public class CommandParserService
    {
        private readonly IServiceProvider _serviceProvider;
        public CommandParserService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        private Type[] RunnerTypes = new[]
        {
            typeof(Runners.TimeRunner),
            typeof(Runners.TimeInRunner),
            typeof(Runners.EddbSystemDistanceRunner),
            typeof(Runners.EddbModuleSearchRunner),
            typeof(Runners.InaraWhoisRunner),
        };

        public async Task<CommandResponse> ProcessAsync(string channelID, string message, string commandTrigger = "/")
        {
            if (message.StartsWith(commandTrigger))
                return await ProcessCommandAsync(channelID, message.Substring(commandTrigger.Length));

            return CommandResponse.Nop;
        }

        public Task<CommandResponse> ProcessCommandAsync(string channelID, string message)
        {
            foreach (var runnerType in RunnerTypes)
                using (var runner = (IRunner)_serviceProvider.GetService(runnerType))
                {
                    if (runner == null)
                        continue;
                    if (!message.StartsWith(runner.Prefix, StringComparison.OrdinalIgnoreCase))
                        continue;
                    if (message.Length > runner.Prefix.Length && message[runner.Prefix.Length] != ' ')
                        continue;
                    return runner.RunAsync(message.Substring(runner.Prefix.Length).Trim().Split(',').Select(s => s.Trim()).ToArray());
                }

            return Task.FromResult(CommandResponse.Nop);
        }
    }
}
