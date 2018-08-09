using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using TheMovieDbBot.Dialogs;
using TheMovieDbBot.Resources;

namespace TheMovieDbBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                var connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                var isTypingReply = activity.CreateReply();
                isTypingReply.Type = ActivityTypes.Typing;
                await connector.Conversations.ReplyToActivityAsync(isTypingReply);

                await Conversation.SendAsync(activity, () => new RootDialog());
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private async Task HandleSystemMessage(Activity activity)
        {
            if (activity.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (activity.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
                var iConversationUpdated = (IConversationUpdateActivity)activity;
                var connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                foreach (var member in iConversationUpdated.MembersAdded ?? Array.Empty<ChannelAccount>())
                {
                    // if the bot is added, then
                    if (member.Id == iConversationUpdated.Recipient.Id)
                    {
                        var reply = activity.CreateReply();
                        var card = GetGreetingCard(iConversationUpdated);
                        reply.Attachments = new List<Attachment> { card.ToAttachment() };
                        await connector.Conversations.ReplyToActivityAsync(reply);
                    }
                }
            }
            else if (activity.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (activity.Type == ActivityTypes.Typing)
            {
                // Handle knowing that the user is typing
            }
            else if (activity.Type == ActivityTypes.Ping)
            {
            }
        }

        private static HeroCard GetGreetingCard(IConversationUpdateActivity iConversationUpdated)
        {
            var card = FactoryDialogControls.GetHeroCard("TheMovieDb Bot",
                null,
                "Hi, Welcome to TheMovieDb Bot where you can answer about movies and actors",
                null,
                null);
            return card;
        }
    }
}