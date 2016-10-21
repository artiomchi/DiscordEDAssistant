using System;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Linq;

namespace FlexLabs.EDAssistant.Services.Commands
{
    public class CommandParserService
    {
        private readonly IServiceProvider _serviceProvider;
        public CommandParserService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<CommandResponse> ProcessAsync(string channelID, string message, string commandTrigger = "/")
        {
            if (message.StartsWith(commandTrigger))
            {
                message = message.Substring(commandTrigger.Length);
                var cut = message.IndexOf(' ');
                var command = cut > 0 ? message.Substring(0, cut) : message;
                var arguments = cut > 0 ? message.Substring(cut + 1) : string.Empty;
                return await ProcessCommandAsync(channelID, command, new[] { arguments });
            }

            return CommandResponse.Nop;
        }

        private async Task<CommandResponse> ProcessCommandAsync(string channelID, string command, string[] arguments)
        {
            switch (command.ToLowerInvariant())
            {
                case "whois":
                    var inaraRunner = _serviceProvider.GetService<Runners.InaraWhoisRunner>();
                    return await inaraRunner.RunAsync(arguments);

                case "time":
                    return Runners.TimeRunner.CurrentTime();

                case "timein":
                    return Runners.TimeRunner.Command_TimeIn(arguments[0], arguments[1]);

                case "dist":
                    using (var eddbDistRunner = _serviceProvider.GetService<Runners.EddbSystemDistanceRunner>())
                    {
                        return eddbDistRunner.Run(arguments[0], arguments[1]);
                    }

                case "modulesnear":
                    using (var eddbDistRunner = _serviceProvider.GetService<Runners.EddbModuleSearchRunner>())
                    {
                        return await eddbDistRunner.RunAsync(arguments[0], arguments.Skip(1).ToArray());
                    }

                case "help":
                    return CommandResponse.Text("{help message}");
            }

            return CommandResponse.Nop;
        }
    }
}
