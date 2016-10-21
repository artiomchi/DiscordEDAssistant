using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlexLabs.EDAssistant.Services.Commands
{
    public class CommandParserService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly LuisProcessor _luisProcessor;
        public CommandParserService(IServiceProvider serviceProvider, LuisProcessor luisProcessor)
        {
            _serviceProvider = serviceProvider;
            _luisProcessor = luisProcessor;
        }

        private List<Type> RunnerTypes = new List<Type>
        {
            typeof(Runners.TimeRunner),
            typeof(Runners.TimeInRunner),
            typeof(Runners.EddbSystemDistanceRunner),
            typeof(Runners.EddbModuleSearchRunner),
            typeof(Runners.InaraWhoisRunner),
        };

        public void RegisterRunner<TRunner>() where TRunner : IRunner
        {
            RunnerTypes.Add(typeof(TRunner));
        }

        public async Task<CommandResponse> ProcessAsync(string channelID, string message, object channelData, string commandTrigger = "/")
        {
            if (message.StartsWith(commandTrigger))
                return await ProcessCommandAsync(channelID, message.Substring(commandTrigger.Length), channelData);

            var command = await _luisProcessor.Process(message);
            if (command != null)
                return await ProcessCommandAsync(channelID, command, channelData);

            return CommandResponse.Error("Sorry, I didn't understand you");
        }

        public Task<CommandResponse> ProcessCommandAsync(string channelID, string message, object channelData)
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
                    var arguments = message.Substring(runner.Prefix.Length).Trim().Split(',').Select(s => s.Trim()).ToArray();
                    return runner.RunAsync(arguments, channelData);
                }

            return Task.FromResult(CommandResponse.Nop);
        }
    }
}
