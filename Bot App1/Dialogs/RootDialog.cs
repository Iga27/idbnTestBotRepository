using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using ParserLibrary;
using System.Collections.Generic;
using System.Linq;

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
            await context.PostAsync("hi from bot. enter the region");

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

            if(reply.Attachments.Count!=0)
            await context.PostAsync(reply);
            else
            await context.PostAsync("ничего не найдено");

            context.Wait(MessageReceivedAsync);

        }

        private static  void ShowHeroCard(IMessageActivity reply)
        {
            var parser = new Parser(region,quantity);
            var flats = parser.GetInfo();
           
            var array = new CardAction[] { new CardAction() { } };
            
              foreach (var f in flats)
             {
                var cardImages = new CardImage[] { new CardImage(url: f.ImageSrc) };

                var cardActions = new CardAction[] {new CardAction()
                {
                    Value = f.Link,
                    Type = "openUrl",
                    Title = "перейти"
                }};
    
                 var card = new HeroCard()
                 {
                     Title = f.Title,
                     Subtitle =region+" region",
                     Images =cardImages,
                     Buttons=cardActions
                 };

                 var cardAttachment = card.ToAttachment();
                 reply.Attachments.Add(cardAttachment);
             }
        }

          

        }
}