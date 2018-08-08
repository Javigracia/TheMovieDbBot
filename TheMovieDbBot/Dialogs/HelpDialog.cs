using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using TheMovieDbBot.Resources;

namespace TheMovieDbBot.Dialogs
{
    [Serializable]
    public class HelpDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync(GetHelp((Activity)context.Activity));
            context.Call(new RootDialog(), null);
        }

        internal static Activity GetHelp(Activity activity)
        {
            var reply = activity.CreateReply();
            reply.Text = "Menu";
            reply.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>
                {
                    new CardAction(ActionTypes.PostBack, "prueba", value: "prueba"),
                    new CardAction(ActionTypes.PostBack, "prueba2", value: "prueba2")
                }
            };
            return reply;
        }
    }
}