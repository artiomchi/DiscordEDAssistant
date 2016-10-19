using Discord.Commands;
using FlexLabs.EDAssistant.Services.Data;
using System.Threading.Tasks;

namespace FlexLabs.EDAssistant.Bot.Commands
{
    public static class KosCommands
    {
        public static void CreateCommands_KosRules(this CommandService commandService)
        {
            commandService.CreateGroup("kos", x =>
            {
                x.CreateCommand()
                    .Description("Check the status of KOS for a user")
                    .AddCheck(Bot.Check_PublicChannel)
                    .Parameter("name")
                    .Do(Commands_Check);

                x.CreateCommand("set")
                    .Description("Set KOS rule for user")
                    .AddCheck(Bot.Check_PublicChannel)
                    .Parameter("name")
                    .Parameter("rule", ParameterType.Unparsed)
                    .Do(Command_Set);

                x.CreateCommand("remove")
                    .Description("Remove KOS rules for user")
                    .AddCheck(Bot.Check_PublicChannel)
                    .Parameter("name")
                    .Do(Command_Remove);
            });
        }

        private static async Task Commands_Check(CommandEventArgs e)
        {
            var user = e.GetArg("name");
            string rule;

            using (var rulesService = Bot.ServiceProvider.GetService(typeof(KosRulesService)) as KosRulesService)
            {
                rule = await rulesService.LoadAsync(e.Server.Id, user);
            }

            if (rule != null)
                await e.Channel.SendMessage($"Yes, {rule}");
            else
                await e.Channel.SendMessage("No");
        }

        private static async Task Command_Set(CommandEventArgs e)
        {
            var user = e.GetArg("name");
            var rule = e.GetArg("rule");

            using (var rulesService = Bot.ServiceProvider.GetService(typeof(KosRulesService)) as KosRulesService)
            {
                await rulesService.SetAsync(e.Server.Id, user, e.User.Id, rule);
            }

            await e.Channel.SendMessage("KOS set");
        }

        private static async Task Command_Remove(CommandEventArgs e)
        {
            var user = e.GetArg("name");

            using (var rulesService = Bot.ServiceProvider.GetService(typeof(KosRulesService)) as KosRulesService)
            {
                await rulesService.DeleteAsync(e.Server.Id, user);
            }

            await e.Channel.SendMessage("KOS removed");
        }
    }
}
