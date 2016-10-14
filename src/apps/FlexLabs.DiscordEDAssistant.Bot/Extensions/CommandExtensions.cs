using Discord.Commands;
using System.Collections.Generic;
using System.Reflection;

namespace FlexLabs.DiscordEDAssistant.Bot.Extensions
{
    public static class CommandExtensions
    {
        private static readonly IList<string> _modCommands = new List<string>();

        public static CommandBuilder ModCommand(this CommandBuilder builder)
        {
            var command = typeof(CommandBuilder).GetField("_command", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(builder) as Command;
            if (command != null)
                _modCommands.Add(command.Text);
            return builder;
        }

        public static bool IsModCommand(this Command command) => _modCommands.Contains(command.Text);
    }
}
