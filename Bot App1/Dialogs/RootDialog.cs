using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using ParserLibrary;
using System.Collections.Generic;

namespace Bot_App1.Dialogs
{
     
    [Serializable]
    public class RootDialog : IDialog<object>
    {

        private static string region;

        private static int quantity;

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("hi from bot.input region");

            context.Wait(RegionReceivedAsync);
        }

        private async Task RegionReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            region = activity.Text;

            await context.PostAsync("number or rooms?");
            context.Wait(FinalReceivedAsync);
        }

        public async Task FinalReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            quantity = int.Parse(activity.Text);

            var reply = context.MakeMessage();
            reply.Attachments = new List<Attachment>();

            ShowHeroCard(reply);

            await context.PostAsync(reply);
            context.Wait(MessageReceivedAsync);

        }

        private static async void ShowHeroCard(IMessageActivity reply)
        {
            var parser = new Parser();
            var flats = parser.GetInfo(quantity, region);

            var array = new CardAction[] { new CardAction() { } };
            
              foreach (var f in flats)
             {
                var cardImages = new CardImage[] { new CardImage(url: f.ImageSrc) };

                var cardActions = new CardAction[] {new CardAction()
                {
                    Value = null,
                    Type = "openUrl",
                    Title = "link"
                }};
    
                 var card = new HeroCard()
                 {
                     Title = f.Title,
                     Subtitle =region+" region",
                     Images =cardImages,
                     Buttons=cardActions
                 };
                 var plAttachment = card.ToAttachment();
                 reply.Attachments.Add(plAttachment);
             }
        }

          

        }
}