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
        private static TownCodeLoader townLoader;

        private static string region;

        //private static int quantity;

        private static string town;

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(RegionReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task RegionReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            region = activity.Text;

           // if()  /////////////////////////////////////
            townLoader = new TownCodeLoader(region);
            townLoader.LoadTowns();

            await context.PostAsync("Введите город");

            context.Wait(TownReceivedAsync);
        }

       /* private async Task TownReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            town = activity.Text;

            await context.PostAsync("Количество комнат?");
            context.Wait(RoomsReceivedAsync);
        }*/

        public async Task TownReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            //var activity = await result as Activity;
            //quantity = int.Parse(activity.Text);
            var activity = await result as Activity;

            town = activity.Text;

            var reply = context.MakeMessage();
            reply.Attachments = new List<Attachment>();

            ShowHeroCard(reply);

            if(reply.Attachments.Count!=0)
            await context.PostAsync(reply);
            else
            await context.PostAsync("ничего не найдено");

            await context.PostAsync("Введите город");
            context.Wait(TownReceivedAsync);

        }

        private static  void ShowHeroCard(IMessageActivity reply)
        {
            var parser = new Parser(region);
            var flats = parser.GetInfo(townLoader.Dictionary[town]);
           
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