using System;
using Microsoft.Extensions.DependencyInjection;
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

        public async Task<CommandResponse> ProcessAsync(String message, String commandTrigger = "/")
        {
            if (message.StartsWith(commandTrigger))
            {
                message = message.Substring(commandTrigger.Length);
                var cut = message.IndexOf(' ');
                var command = cut > 0 ? message.Substring(0, cut) : message;
                var arguments = cut > 0 ? message.Substring(cut + 1) : string.Empty;
                return await ProcessCommandAsync(command, new[] { arguments });
            }

            return CommandResponse.Nop;
        }

        private async Task<CommandResponse> ProcessCommandAsync(string command, string[] arguments)
        {
            switch (command.ToLowerInvariant())
            {
                case "whois":
                    var inaraRunner = _serviceProvider.GetService<Runners.InaraWhoisRunner>();
                    return await inaraRunner.RunAsync(arguments);

                case "help":
                    return CommandResponse.Text("{help message}");
            }

            return CommandResponse.Nop;
        }
    }
}
