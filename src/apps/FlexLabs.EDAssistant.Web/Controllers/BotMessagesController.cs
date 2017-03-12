using FlexLabs.EDAssistant.Services.Commands;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FlexLabs.EDAssistant.Web.Controllers
{
    [BotAuthentication]
    [Route("api/[controller]")]
    public class BotMessagesController : Controller
    {
        private readonly CommandParserService _commandProcessorService;
        public BotMessagesController(CommandParserService commandProcessorService)
        {
            _commandProcessorService = commandProcessorService;
        }

        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<IActionResult> Post([FromBody]Activity activity)
        {
            ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
            try
            {
                if (activity.Type == ActivityTypes.Message && !string.IsNullOrWhiteSpace(activity.Text))
                {
                    var result = await _commandProcessorService.ProcessAsync(activity.ChannelId, activity.Text);
                    foreach (var content in result.Contents)
                        await connector.Conversations.ReplyToActivityAsync(activity.CreateReply(content.Format(activity.ChannelId)));
                }
                else
                {
                    HandleSystemMessage(activity);
                }
            }
            catch (Exception ex)
            {
                await connector.Conversations.ReplyToActivityAsync(activity.CreateReply("ERROR!" + ex.Message));
            }
            var response = Ok();
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}
